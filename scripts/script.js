window.sectionObserver = {
    observer: null,

    observe(dotNetRef, selector) {
        const sections = document.querySelectorAll(selector);

        this.observer = new IntersectionObserver((entries) => {
            const visible = entries
                .filter(entry => entry.isIntersecting)
                .sort((a, b) => b.intersectionRatio - a.intersectionRatio)[0];

            if (visible) {
                dotNetRef.invokeMethodAsync(
                    "SetActiveSection",
                    visible.target.id
                );
            }
        }, {
            threshold: [0.45, 0.6, 0.75]
        });

        sections.forEach(section => this.observer.observe(section));
    },

    disconnect() {
        this.observer?.disconnect();
        this.observer = null;
    }
};