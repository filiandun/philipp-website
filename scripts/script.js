window.sectionObserver = {
    observer: null,

    visibleSections: new Map(),

    observe(dotNetRef, selector) {
        const sections = document.querySelectorAll(selector);
        const thresholds = Array.from({ length: 21 }, (_, i) => i * 0.05);

        this.observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    this.visibleSections.set(entry.target.id, entry.intersectionRect.height);
                } else {
                    this.visibleSections.delete(entry.target.id);
                }
            });

            if (this.visibleSections.size > 0) {
                let maxId = null;
                let maxHeight = 0;

                for (let [id, height] of this.visibleSections.entries()) {
                    if (height > maxHeight) {
                        maxHeight = height;
                        maxId = id;
                    }
                }

                if (maxId) {
                    dotNetRef.invokeMethodAsync("SetActiveSection", maxId);
                }
            }
        }, {
            threshold: thresholds
        });

        sections.forEach(section => this.observer.observe(section));
    },

    disconnect() {
        this.observer?.disconnect();
        this.observer = null;
        this.visibleSections.clear();
    }
};