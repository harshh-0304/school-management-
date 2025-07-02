// Custom JavaScript for the homepage (extracted from Index.cshtml)

document.addEventListener('DOMContentLoaded', () => {
    const elements = document.querySelectorAll('.transition');
    elements.forEach(el => {
        el.style.transition = 'all 0.3s ease-in-out';
    });

    document.querySelectorAll('.hover-scale-105').forEach(el => {
        el.onmouseover = () => el.style.transform = 'scale(1.05)';
        el.onmouseout = () => el.style.transform = 'scale(1)';
    });

    document.querySelectorAll('.hover-shadow-lg').forEach(el => {
        el.onmouseover = () => el.classList.add('shadow-lg');
        el.onmouseout = () => el.classList.remove('shadow-lg');
    });

    document.querySelectorAll('.hover-translate-y-n2').forEach(el => {
        el.onmouseover = () => el.style.transform = 'translateY(-0.5rem)';
        el.onmouseout = () => el.style.transform = 'translateY(0)';
    });

    document.querySelectorAll('.hover-bg-light').forEach(el => {
        el.onmouseover = () => el.classList.add('bg-light');
        el.onmouseout = () => el.classList.remove('bg-light');
    });

    // Particles.js Initialization
    // Ensure particles.js library is loaded BEFORE this script runs
    if (typeof particlesJS !== 'undefined') {
        particlesJS('particles-js', {
            "particles": {
                "number": {
                    "value": 80,
                    "density": {
                        "enable": true,
                        "value_area": 900
                    }
                },
                "color": {
                    "value": "#ffffff"
                },
                "shape": {
                    "type": "circle",
                    "stroke": {
                        "width": 0,
                        "color": "#000000"
                    },
                    "polygon": {
                        "nb_sides": 5
                    },
                    "image": {
                        "src": "img/github.svg", // This image won't be found unless you provide it
                        "width": 100,
                        "height": 100
                    }
                },
                "opacity": {
                    "value": 0.5,
                    "random": false,
                    "anim": {
                        "enable": false,
                        "speed": 1,
                        "opacity_min": 0.1,
                        "sync": false
                    }
                },
                "size": {
                    "value": 5,
                    "random": true,
                    "anim": {
                        "enable": false,
                        "speed": 50,
                        "size_min": 0.1,
                        "sync": false
                    }
                },
                "line_linked": {
                    "enable": true,
                    "distance": 120,
                    "color": "#ffffff",
                    "opacity": 0.4,
                    "width": 1
                },
                "move": {
                    "enable": true,
                    "speed": 6,
                    "direction": "none",
                    "random": false,
                    "straight": false,
                    "out_mode": "out",
                    "bounce": false,
                    "attract": {
                        "enable": false,
                        "rotateX": 600,
                        "rotateY": 1200
                    }
                }
            },
            "interactivity": {
                "detect_on": "canvas",
                "events": {
                    "onhover": {
                        "enable": true,
                        "mode": "grab"
                    },
                    "onclick": {
                        "enable": true,
                        "mode": "push"
                    },
                    "resize": true
                },
                "modes": {
                    "grab": {
                        "distance": 140,
                        "line_linked": {
                            "opacity": 1
                        }
                    },
                    "bubble": {
                        "distance": 400,
                        "size": 40,
                        "duration": 2,
                        "opacity": 8,
                        "speed": 3
                    },
                    "repulse": {
                        "distance": 200,
                        "duration": 0.4
                    },
                    "push": {
                        "particles_nb": 4
                    },
                    "remove": {
                        "particles_nb": 2
                    }
                }
            },
            "retina_detect": true
        });
    } else {
        console.warn("Particles.js library not found. Ensure particles.min.js is loaded before site.js.");
    }
});
