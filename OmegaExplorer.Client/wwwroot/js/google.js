window.googleSignIn = async function (googleClientId) {
    console.log('[Google.JS] : googleSignIn called with client id: ' + googleClientId);
    
    // Check if Google API is loaded
    const waitForGoogleApi = (maxAttempts = 10, interval = 500) => {
        return new Promise((resolve, reject) => {
            let attempts = 0;
            
            const checkGoogleApi = () => {
                attempts++;
                console.log(`[Google.JS] : Checking Google API (attempt ${attempts}/${maxAttempts})`);
                
                if (typeof google !== 'undefined' && google.accounts && google.accounts.id) {
                    console.log('[Google.JS] : Google API successfully loaded');
                    resolve();
                } else if (attempts >= maxAttempts) {
                    reject('Google API not available after multiple attempts');
                } else {
                    setTimeout(checkGoogleApi, interval);
                }
            };
            
            checkGoogleApi();
        });
    };
    
    try {
        await waitForGoogleApi();
        
        return new Promise((resolve, reject) => {
            const timeout = setTimeout(() => {
                console.log('[Google.JS] : Timeout: No token received within 20 seconds');
                reject('Timeout: No token received within 20 seconds');
            }, 20000);

            // Ensure the Google sign-in button is always visible and properly styled
            const renderButton = () => {
                let buttonDiv = document.getElementById("googleSignInDiv");
                
                // Create the button div if it doesn't exist
                if (!buttonDiv) {
                    console.log('[Google.JS] : Creating googleSignInDiv as it was not found');
                    buttonDiv = document.createElement("div");
                    buttonDiv.id = "googleSignInDiv";
                    buttonDiv.style.position = "fixed";
                    buttonDiv.style.top = "50%";
                    buttonDiv.style.left = "50%";
                    buttonDiv.style.transform = "translate(-50%, -50%)";
                    buttonDiv.style.zIndex = "1000";
                    document.body.appendChild(buttonDiv);
                }
                
                console.log('[Google.JS] : Rendering Google Sign-In button');
                
                // Clear any existing content first
                buttonDiv.innerHTML = '';
                
                // Make sure the button is visible with appropriate styling
                buttonDiv.style.display = "block";
                buttonDiv.style.margin = "10px auto";
                buttonDiv.style.textAlign = "center";
                
                google.accounts.id.renderButton(buttonDiv, {
                    theme: "outline",
                    size: "large",
                    type: "standard",
                    text: "signin_with",
                    width: 280, // Increased width for better visibility
                    logo_alignment: "center"
                });
                
                // Add a message to guide users
                const helpText = document.createElement('div');
                helpText.innerText = "Click to sign in with Google";
                helpText.style.marginTop = "5px";
                helpText.style.fontSize = "12px";
                helpText.style.color = "#666";
                buttonDiv.appendChild(helpText);
                
                console.log('[Google.JS] : Button rendered successfully');
            };

            // Initialize the Google Identity Services with FedCM enabled
            google.accounts.id.initialize({
                auto_select: false,
                client_id: googleClientId,
                use_fedcm: true, // Enable FedCM
                cancel_on_tap_outside: false, // Don't cancel when clicking outside
                callback: (response) => {
                    clearTimeout(timeout);
                    if (response.credential) {
                        console.log('[Google.JS] : Google ID token received');
                        
                        // Hide the sign-in button after successful authentication
                        const buttonDiv = document.getElementById("googleSignInDiv");
                        if (buttonDiv) {
                            // Fade out animation
                            buttonDiv.style.transition = "opacity 0.5s ease";
                            buttonDiv.style.opacity = "0";
                            
                            // Remove from DOM after animation completes
                            setTimeout(() => {
                                if (buttonDiv && buttonDiv.parentNode) {
                                    buttonDiv.parentNode.removeChild(buttonDiv);
                                }
                            }, 500);
                        }
                        
                        resolve(response.credential);
                    } else {
                        console.log('[Google.JS] : Response without credential');
                        reject('No token received');
                    }
                },
                error_callback: (error) => {
                    console.error('[Google.JS] : Google Sign-In error:', error);
                    clearTimeout(timeout);
                    reject(`Google Sign-In error: ${error}`);
                }
            });

            // Always render the button first to ensure a login option
            renderButton();

            // Wait a short delay before displaying the prompt
            setTimeout(() => {
                try {
                    console.log('[Google.JS] : Attempting to display Google prompt');
                    
                    // FedCM compatible approach
                    google.accounts.id.prompt((notification) => {
                        console.log('[Google.JS] : Prompt notification received', notification);
                        
                        // Handle specific notification states for better user experience
                        if (notification.isNotDisplayed() || 
                            notification.isSkippedMoment() || 
                            notification.isDismissedMoment() ||
                            (notification.j && notification.j === 'suppressed_by_user')) {
                                
                            console.log('[Google.JS] : Prompt not displayed or was suppressed/dismissed. Reason:', 
                                notification.getNotDisplayedReason() || 
                                notification.getSkippedReason() || 
                                notification.getDismissedReason() || 
                                notification.j);
                                
                            // Focus on the button as fallback
                            const buttonDiv = document.getElementById("googleSignInDiv");
                            if (buttonDiv) {
                                // Highlight the button to draw attention
                                buttonDiv.style.animation = "pulse 2s infinite";
                                
                                // Add pulse animation style if it doesn't exist
                                if (!document.getElementById('google-signin-styles')) {
                                    const style = document.createElement('style');
                                    style.id = 'google-signin-styles';
                                    style.innerHTML = `
                                        @keyframes pulse {
                                            0% { box-shadow: 0 0 0 0 rgba(66, 133, 244, 0.4); }
                                            70% { box-shadow: 0 0 0 10px rgba(66, 133, 244, 0); }
                                            100% { box-shadow: 0 0 0 0 rgba(66, 133, 244, 0); }
                                        }
                                    `;
                                    document.head.appendChild(style);
                                }
                            }
                            
                            // Re-render button to ensure it's visible
                            renderButton();
                        }
                    });
                } catch (promptError) {
                    console.error('[Google.JS] : Error displaying prompt:', promptError);
                    // Ensure the button is visible as fallback
                    renderButton();
                }
            }, 500);
        });
    } catch (error) {
        console.error('[Google.JS] : Initialization error:', error);
        throw error;
    }
};
