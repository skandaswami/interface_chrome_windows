
//Adding event listeners to the buttons.
document.getElementById("restart").addEventListener("click",verifyCode);
document.getElementById("startChrome").addEventListener("click",relayStartChromeToBackground)
document.getElementById("enableButton").addEventListener("click",relayEnableButtonToBackground)
document.getElementById("disableButton").addEventListener("click",relayDisableButtonToBackground)
document.getElementById("deletePrinters").addEventListener("click",relayDeletePrintersToBackground)


//Sends a message to background.js to restart windows.
function relayRestartToBackground(){
	chrome.runtime.sendMessage({text:"restart windows"},function(response){
		console.log("Told background.js to restart windows.");
	});
}
function relayDeletePrintersToBackground(){
	chrome.runtime.sendMessage({text:"delete printers"},function(response){
		console.log("Told background.js to delete printers.");
	});
}

//Sends a message to background.js to start chrome.
function relayStartChromeToBackground(){
	chrome.runtime.sendMessage({text:"start chrome"},function(response){
		console.log("Told background.js to start chrome.");
	});
}

//Sends a message to enable the windows home button.
function relayEnableButtonToBackground(){
	chrome.runtime.sendMessage({text:"enable button"},function(response){
		console.log("Told background.js to enable the windows button.");
	});
}

//Sends a message to disable the windows home button.
function relayDisableButtonToBackground(){
	chrome.runtime.sendMessage({text:"disable button"},function(response){
		console.log("Told background.js to disable the windows button.");
	});
}
//Function that verifies whether the code entered in the numeric field is the right one.
function verifyCode(){
	var SecretCodeThatWillNotBeHardcoded='1234';
	var code=document.getElementById("numeric_code1").value;
	if(code==SecretCodeThatWillNotBeHardcoded)
	{
		relayRestartToBackground();
	}
}
