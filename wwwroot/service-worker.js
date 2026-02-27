// OptionA Tuner — Service Worker for offline PWA support
// Version is updated on each deploy to bust the cache
const CACHE_VERSION = 'optiona-tuner-v4';

// Resources to pre-cache on install
const PRECACHE_URLS = [
    '/',
    '/index.html',
    '/css/app.css',
    '/favicon.svg',
    '/logo.svg',
    '/manifest.webmanifest',
    '/js/audioInterop.js',
    '/js/pitchProcessor.js'
];

// Install: pre-cache shell resources
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_VERSION).then(cache => {
            return cache.addAll(PRECACHE_URLS);
        }).then(() => self.skipWaiting())
    );
});

// Activate: clean up old caches
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(keys => {
            return Promise.all(
                keys
                    .filter(key => key !== CACHE_VERSION)
                    .map(key => caches.delete(key))
            );
        }).then(() => self.clients.claim())
    );
});

// Fetch: network-first for navigation, cache-first for assets
self.addEventListener('fetch', event => {
    const request = event.request;

    // Skip non-GET requests
    if (request.method !== 'GET') return;

    // For navigation requests (HTML pages): network-first
    if (request.mode === 'navigate') {
        event.respondWith(
            fetch(request)
                .then(response => {
                    const clone = response.clone();
                    caches.open(CACHE_VERSION).then(cache => cache.put(request, clone));
                    return response;
                })
                .catch(() => caches.match(request) || caches.match('/index.html'))
        );
        return;
    }

    // For _framework assets (WASM, DLLs, JS): cache-first (they have fingerprinted names)
    if (request.url.includes('/_framework/')) {
        event.respondWith(
            caches.match(request).then(cached => {
                if (cached) return cached;
                return fetch(request).then(response => {
                    const clone = response.clone();
                    caches.open(CACHE_VERSION).then(cache => cache.put(request, clone));
                    return response;
                });
            })
        );
        return;
    }

    // For other assets (CSS, JS, images): stale-while-revalidate
    event.respondWith(
        caches.match(request).then(cached => {
            const fetchPromise = fetch(request).then(response => {
                const clone = response.clone();
                caches.open(CACHE_VERSION).then(cache => cache.put(request, clone));
                return response;
            }).catch(() => cached);

            return cached || fetchPromise;
        })
    );
});
