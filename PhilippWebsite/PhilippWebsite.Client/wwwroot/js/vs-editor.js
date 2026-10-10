window.vsCodeEditor = {
    initHighlight: function (rootElement) {
        if (!rootElement || typeof rootElement.querySelector !== 'function') {
            rootElement = document.querySelector('.vs-editor-root');
        }

        if (!rootElement) return;

        const layersContainer = rootElement.querySelector('.vs-editor-layers');
        if (!layersContainer) return;

        const LINE_HEIGHT = 20;
        let currentRowIndex = -1;

        rootElement.addEventListener('mousemove', (e) => {
            const rows = layersContainer.children;
            const rect = layersContainer.getBoundingClientRect();

            const y = e.clientY - rect.top;
            const index = Math.floor(y / LINE_HEIGHT);

            if (index === currentRowIndex) return;

            if (index < 0 || index >= rows.length) {
                clearHighlight(rows);
                return;
            }

            clearHighlight(rows);
            rows[index].classList.add('hovered');
            currentRowIndex = index;
        });

        rootElement.addEventListener('mouseleave', () => {
            if (layersContainer) {
                clearHighlight(layersContainer.children);
            }
        });

        function clearHighlight(rows) {
            if (currentRowIndex >= 0 && currentRowIndex < rows.length) {
                rows[currentRowIndex].classList.remove('hovered');
            }
            currentRowIndex = -1;
        }
    }
};