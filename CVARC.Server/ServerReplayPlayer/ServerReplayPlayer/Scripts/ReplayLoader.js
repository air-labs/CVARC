ReplayLoader = {
dt:		0,	size:	0,	pos:	0,	delayTime:	0,
data:	[],	types: ["Body", "Ball", "Box", "Cylinder"],
order:	["x", "y", "z", "Z", "Y", "X", "parent", "color"],
typesOrder: [[], ["radius"], ["xSize", "ySize", "zSize", "texture"], ["rTop", "rBottom", "height"]],
ready:	false,

load: function (url) {
    $.ajax({
        url: url,
        type: "Get"
    }).done(function(data) {
        ReplayLoader.data = JSON.parse(data);
        ReplayLoader.dt = ReplayLoader.data[ReplayLoader.pos++] * 100;
        ReplayLoader.size = ReplayLoader.data.length;
        ReplayLoader.ready = true;
    });
},
setDelayTime:	function(time) {
	 ReplayLoader.delayTime = Math.round( time*1000 );
},

isReady:	function() { 
	 return ReplayLoader.ready;
},

getNext:	function() {
	 var pos = ReplayLoader.pos++;
	 var time = Math.round( (pos-1)*ReplayLoader.dt ) - ReplayLoader.delayTime;

	 var array = ReplayLoader.data[pos].map(ReplayLoader.convert);
	 
	 return [time, array];
},
convert:	function(str) {
	 var array = str.split(",");
	 var type = array.shift();
	 var object = {};
	 
	 if(type > 0)
		{
		 object.id = type;
		 ReplayLoader.readEdit(object, array);
		}
		else
		{
		 object.id = array.shift();
		 ReplayLoader.readEdit(object, array);
		 ReplayLoader.readCreate(object, array, -type);
		}
	 return object;
},
readEdit:	function(object, array) {
	 var order = ReplayLoader.order;
	 for(var i in order)
		{
		 var elem = array.shift();
		 if(elem && elem != "")
			object[order[i]] = elem;
		}
},
readCreate:	function(object, array, type) {
	 var order = ReplayLoader.typesOrder[type];
	 object.name = ReplayLoader.types[type];
	 for(var i in order)
		{
		 var elem = array.shift();
		 if(elem && elem != "")
			object[order[i]] = elem;
		}
},
hasNext:	function() {
	 return ReplayLoader.pos < ReplayLoader.size;
},
};