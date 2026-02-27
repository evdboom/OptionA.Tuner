// OptionA Tuner — Application Entry Point
// Registers the service worker and imports the top-level component.

import '../components/tuner-app.js';

// Register service worker for offline PWA support
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('./service-worker.js');
}
