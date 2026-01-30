window.clipboardInterop = {
    copyText: async function (text) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (err) {
            console.error('Failed to copy text: ', err);
            return false;
        }
    }
};

window.scrollInterop = {
    observers: {},

    observeElement: function (elementId, dotNetHelper) {
        const element = document.getElementById(elementId);
        if (!element) {
            console.warn('Element not found for scroll observation:', elementId);
            return;
        }

        // Disconnect any existing observer for this element
        if (this.observers[elementId]) {
            this.observers[elementId].disconnect();
        }

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotNetHelper.invokeMethodAsync('OnScrolledToBottom');
                    observer.disconnect();
                    delete this.observers[elementId];
                }
            });
        }, {
            root: null,
            rootMargin: '0px',
            threshold: 0.1
        });

        observer.observe(element);
        this.observers[elementId] = observer;
    },

    dispose: function (elementId) {
        if (this.observers[elementId]) {
            this.observers[elementId].disconnect();
            delete this.observers[elementId];
        }
    }
};
