function showReplay(div, url, delayTime) {
	 ReplayLoader.load(url);
	 if(delayTime)
		ReplayLoader.setDelayTime(delayTime);
	 
	 ReplayWindow.addScene(div);
	 ReplayWindow.addFPS(div);
	 ReplayWindow.addEvents(div);
	 
	 ReplayWindow.start();
}