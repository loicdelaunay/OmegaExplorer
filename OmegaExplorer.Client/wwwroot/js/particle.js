window.triggerConfetti = (elementId) => {
    console.log("confetti ! : " + tsParticles);
    tsParticles.load({
        id: "tsparticles",
        options: {
            fullScreen: {
                enable: true,
                zIndex: 9999 // Or any large number to stay on top
            },            
            emitters: [
                {
                    life: {
                        duration: 10,
                        count: 10,
                    },
                    position: {
                        x: 100,
                        y: 100,
                    },
                    particles: {
                        move: {
                            direction: "bottom-right",
                        },
                    },
                },
            ],
            preset: "confetti",
        },
    });
};