mergeInto(LibraryManager.library, {

	/**
	* GetUser: function() {
	*	const { parent } = window;
	*	parent.postMessage({ playdeck: { method: "getUser" } }, "*");
	*
	*	window.addEventListener("message", ({ data }) => {
	*		const playdeck = data?.playdeck;
	*		if (!playdeck) return;
	*
	*		if (playdeck.method === "getUser") {
	*			window.playdeckUser = playdeck.value;
	*		}
	*	});
	*},
	*/

	/**
	* SetScore: function(score, force) {
	*	const { parent } = window;
	*
	*	parent.postMessage(
	*		{ playdeck:
	*			{
	*				method: "setScore",
	*				value: score,
	*				isForce: force,
	*			}
	*		}
	*	, "*");
	*},
	*/
});