mergeInto(LibraryManager.library, {
	VibrateArray_VibrationWebGL: function(array, size) {
		if (!navigator.vibrate) return false;
        return navigator.vibrate(HEAPU32.subarray(array >> 2, (array >> 2) + size));
	},
	Vibrate_VibrationWebGL: function(value) {
		if (!navigator.vibrate) return false;
		return navigator.vibrate(value);
	},
	isSupported_VibrationWebGL: function() {
		return navigator.vibrate ? true : false;
	}
});