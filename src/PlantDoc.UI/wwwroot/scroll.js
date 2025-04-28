window.scrollHelpers = {
    scrollToBottom: function(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            // Smooth scroll with timeout to ensure DOM stability
            setTimeout(() => {
                element.scrollTo({
                    top: element.scrollHeight,
                    behavior: 'smooth'
                });
            }, 50);
        }
    }
};