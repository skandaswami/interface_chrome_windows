
//This is the background.js file of the chrome extension. 
//It handles the communication with the Native Host App and content.js

//Sends a message to the native host app to restart the system.
function restartComputer(port){  
	port.postMessage({ text: "restart" });
}
//Sends a message to the native host app to start chrome in kiosk mode.
function startChrome(port){
	port.postMessage({ text: "start chrome" });
} 
//Sends a message to the native host app to enable the use of the windows home button.
function enableWindowsKey(port){
	port.postMessage({ text: "enable button" });
} 
//Sends a message to the native host app to disable the use of the windows home button.
function disableWindowsKey(port){
	port.postMessage({ text: "disable button" });
} 
//Function to delete all the local printers
function deleteLocalPrinters(port){
	port.postMessage({ text: "delete printers" });
} 
//Listener listens for calls from content.js and other parts of the extension.
chrome.runtime.onMessage.addListener(
function(request,sender,sendResponse){
	console.log("Background.js was contacted");
 	//Establishing a connection between the extension and the Native Host using a port.
 	var port = chrome.runtime.connectNative('com.prime3.interface');
	port.onMessage.addListener(function(msg) {
  	console.log("Received" + msg['data']);
	});
	port.onDisconnect.addListener(function() {
  	console.log("Disconnected"+chrome.runtime.lastError.message);
	});
	//Calling the functions based on the type of request.	
 	switch(request.text){
	 	case "restart windows":{
	 		console.log("Request to restart the computer was called.");
	 		restartComputer(port);
		}
		break;
		case "delete printers":{
	 		console.log("Request to delete the printers was called.");
	 		deleteLocalPrinters(port);
		}
		break;
		case "start chrome":{
	 		console.log("Request to start chrome in kiosk mode was called.");
	 		startChrome(port);
		}
		break;
		case "enable button":{
			console.log("Request to enable the windows key was called");
			enableWindowsKey(port);
		}
		break;
		case "disable button":{
			console.log("Request to dsiable the windows key was called");
			disableWindowsKey(port);
		}

	}
});


