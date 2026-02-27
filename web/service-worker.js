// OptionA Tuner — Service Worker (Lit version)
// Offline-first PWA with versioned cache.

const CACHE_VERSION = 'optiona-tuner-lit-v1';

const PRECACHE_URLS = [
    '/',
    '/index.html',
    '/css/app.css',
    '/vendor/lit.js',
    '/js/app.js',
    '/js/audio-service.js',
    '/js/audio-worklet.js',
    '/js/instruments.js',
    '/js/note-mapper.js',
    '/js/pitch-detector.js',
    '/js/storage-service.js',
    '/components/tuner-app.js',
    '/components/tuning-gauge.js',
    '/components/note-display.js',
    '/components/string-indicator.js',
    '/components/instrument-picker.js',
    '/favicon.svg',
    '/logo.svg',
    '/manifest.webmanifest',
];

// Install: pre-cache shell resources
self.addEventListener('install', (event) => {
    event.waitUntil(
        caches
            .open(CACHE_VERSION)
            .then((cache) => cache.addAll(PRECACHE_URLS))
            .then(() => self.skipWaiting())
    );
});

// Activate: clean up old caches
self.addEventListener('activate', (event) => {
    event.waitUntil(
        caches
            .keys()
            .then((keys) =>
                Promise.all(keys.filter((k) => k !== CACHE_VERSION).map((k) => caches.delete(k)))
            )
            .then(() => self.clients.claim())
    );
});

// Fetch: network-first for navigation, stale-while-revalidate for assets
self.addEventListener('fetch', (event) => {
    const request = event.request;
    if (request.method !== 'GET') return;

    // Navigation: network-first with offline fallback
    if (request.mode === 'navigate') {
        event.respondWith(
            fetch(request)
                .then((response) => {
                    const clone = response.clone();
                    caches.open(CACHE_VERSION).then((cache) => cache.put(request, clone));
                    return response;
                })
                .catch(() => caches.match(request).then((r) => r || caches.match('/index.html')))
        );
        return;
    }

    // All other assets: stale-while-revalidate
    event.respondWith(
        caches.match(request).then((cached) => {
            const fetchPromise = fetch(request)
                .then((response) => {
                    const clone = response.clone();
                    caches.open(CACHE_VERSION).then((cache) => cache.put(request, clone));
                    return response;
                })
                .catch(() => cached);

            return cached || fetchPromise;
        })
    );
});
