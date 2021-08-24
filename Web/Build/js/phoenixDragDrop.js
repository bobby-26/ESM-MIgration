document.onmousemove = mouseMove;
document.onmouseup   = mouseUp;

var dragObject  = null;
var mouseOffset = null;
var dropTargets = [];

function mouseMove(ev){
	ev           = ev || window.event;
	var mousePos = mouseCoords(ev);

	if(dragObject){
		dragObject.style.position = 'absolute';
		dragObject.style.top      = mousePos.y - mouseOffset.y;
		dragObject.style.left     = mousePos.x - mouseOffset.x;

		return false;
	}
}

function mouseCoords(ev){
	if(ev.pageX || ev.pageY){
		return {x:ev.pageX, y:ev.pageY};
	}
	return {
		x:ev.clientX + document.body.scrollLeft - document.body.clientLeft,
		y:ev.clientY + document.body.scrollTop  - document.body.clientTop
	};
}

function makeClickable(object){    
	object.onmousedown = function(){
		dragObject = this;
	}
}


function getMouseOffset(target, ev){
	ev = ev || window.event;

	var docPos    = getPosition(target);
	var mousePos  = mouseCoords(ev);
	return {x:mousePos.x + 2, y:mousePos.y + 2};
}

function getPosition(e){
	var left = 0;
	var top  = 0;

	while (e.offsetParent){
		left += e.offsetLeft;
		top  += e.offsetTop;
		e     = e.offsetParent;
	}

	left += e.offsetLeft;
	top  += e.offsetTop;

	return {x:left, y:top};
}

function makeDraggable(item){
	if(!item) return;
	item.onmousedown = function(ev){
		dragObject  = this;
		mouseOffset = getMouseOffset(this, ev);
		return false;
	}
}

/*
All code from the previous example is needed with the exception
of the mouseUp function which is replaced below
*/

function addDropTarget(dropTarget){
	dropTargets.push(dropTarget);
}

function mouseUp(ev){
	ev           = ev || window.event;
	var mousePos = mouseCoords(ev);

	for(var i=0; i<dropTargets.length; i++){
		var curTarget  = dropTargets[i];
		var targPos    = getPosition(curTarget);
		var targWidth  = parseInt(curTarget.offsetWidth);
		var targHeight = parseInt(curTarget.offsetHeight);

		if(
			(mousePos.x > targPos.x)                &&
			(mousePos.x < (targPos.x + targWidth))  &&
			(mousePos.y > targPos.y)                &&
			(mousePos.y < (targPos.y + targHeight))){
				// dragObject was dropped onto curTarget!
		}
	}
	dragObject   = null;
}
