window.updateDiagramExtension = function() {
    var elements = document.querySelectorAll('.diagram-canvas');

    if (elements.length === 0) {
        return;
    }

    console.log('[Include.js] : detect ' + elements.length + ' diagram, enable diagram extension.');

    // For each element, add event listeners for 'wheel' and 'touchmove' events.
    elements.forEach(function (element) {
        element.addEventListener('wheel', function (e) {
            e.preventDefault();

        }, {passive: false});

        element.addEventListener('touchmove', function (e) {
            if (e.scale !== 1) {
                e.preventDefault();
            }
        }, {passive: false});
    });
}

window.registerResizeHandler = function (dotNetObject) {
    console.log("[Include.js] : registering resize handler");

    let resizeTimer;
    window.addEventListener('resize', () => {
        // Cancels the current timer (if it exists)
        clearTimeout(resizeTimer);

        // Restarts the timer so that it counts down after 1 second
        resizeTimer = setTimeout(() => {
            dotNetObject.invokeMethodAsync('WindowResized');
        }, 1000);
    });
};

window.saveAsFile = function (filename, contentType, data) {
    console.log("[Include.js] : save as file " + filename);
    const blob = new Blob([data], {type: contentType});

    if (navigator.msSaveBlob) {
        // For Internet Explorer and Microsoft Edge browsers
        navigator.msSaveBlob(blob, filename);
    } else {
        // For other browsers
        const link = document.createElement('a');
        if (link.download !== undefined) {
            const url = URL.createObjectURL(blob);
            link.setAttribute('href', url);
            link.setAttribute('download', filename);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
        }
    }
};