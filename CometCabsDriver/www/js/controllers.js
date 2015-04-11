angular.module('starter.controllers', [])

.controller('DashCtrl', function($scope) {})

.controller('ChatsCtrl', function($scope, Chats) {
  $scope.chats = Chats.all();
  $scope.remove = function(chat) {
    Chats.remove(chat);
  }
})

.controller('ChatDetailCtrl', function($scope, $stateParams, Chats) {
  $scope.chat = Chats.get($stateParams.chatId);
})

.controller('AccountCtrl', function($scope) {
  $scope.settings = {
    enableFriends: true
  };
})

.controller('LoginCtrl', function($scope, LoginService, $ionicPopup, $state) {
    $scope.data = {};
 
    $scope.login = function() {
        LoginService.loginUser($scope.data.username, $scope.data.password).success(function(data) {
            $state.go('driver');
        }).error(function(data) {
            var alertPopup = $ionicPopup.alert({
                title: 'Login failed!',
                template: 'Please check your credentials!'
            });
        });
    }
})

.controller('MapCtrl', function($scope, $ionicLoading, $compile) {
			
	var rutfordMarker; 
	var phase1SouthMarker;
	var chicago = new google.maps.LatLng(41.850033, -87.6500523);
	var map;
	var full;
	var fullControl;
	var fullUI;	
	var totalRiders;
	var totalText;
	function initialize() {
		/*Latitude and longitude for the school. Don't know whether we could use more precision */
        var site = new google.maps.LatLng(32.986,-96.750);
		full = false;
		totalRiders = 0;
        var mapOptions = {
          streetViewControl:true,
          center: site,
          zoom: 16, /*This can be played with. It's either +-1 that we can see building shapes */
        };
		/*The element references the div id in index.html*/
        map = new google.maps.Map(document.getElementById("map"),
            mapOptions);
        setRouteCoordinates(); //Sets the details for routes
		setCabMarkers();
		
		/*Sets a marker for the current position */
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(function(position) {
			var currentPosition = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
			var marker = new google.maps.Marker({
				position: currentPosition,
				map: map,
				title:"You are here"
				});		
			});
		} else {
			alert("Geolocation is not supported by this browser.");
		}
		//Refer to https://developers.google.com/maps/documentation/javascript/controls 
		//for info on control positioning
		var fullControlDiv = document.createElement('div');		
		fullControl = new FullControl(fullControlDiv, map);
		fullControlDiv.index = 1;
		map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(fullControlDiv);
		
		var incrementRiderDiv = document.createElement('div');
		var incrementRider = new IncrementRiderControl(incrementRiderDiv, map);
		incrementRiderDiv.index = 1;
		map.controls[google.maps.ControlPosition.RIGHT_CENTER].push(incrementRiderDiv);
		
		var decrementRiderDiv = document.createElement('div');
		var decrementRider = new DecrementRiderControl(decrementRiderDiv, map);
		decrementRiderDiv.index = 2;
		map.controls[google.maps.ControlPosition.RIGHT_CENTER].push(decrementRiderDiv);
		
		var totalDiv = document.createElement('div');
		var totalRiderControl = new RiderTotalsView(totalDiv, map);
		totalDiv.index = 1;
		map.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(totalDiv);
  
		google.maps.event.addListener(map, 'zoom_changed', function() {
     if (map.getZoom() < 15) map.setZoom(15);
   });
       
  }
  function getRouteJSON() {
	$.ajax({
		type: 'Get',
		url: 'http://cometcabsservices.azurewebsites.net/CometCabsServices.svc/basic/GetRouteData',
		success: function() {
			alert("Success!");
		}
    });
  
  
  
  }
  function setRouteCoordinates() {
	/*
		The JSON for routes:
		var jsonRoute = {
			"name": <string>,
			"color": <in the form #000000>,
			[
			{"latitude": <double>, "longitude": <double> } ]
		}	
			With two or more points in the array.	
	*/
	
	var routes = [
		{
			"name": "McDermott",
			"color": '#003986',
			"path": [
				{"latitude": 32.990034, "longitude": -96.744273},
				{"latitude": 32.989341, "longitude": -96.745367},
				{"latitude": 32.988540, "longitude": -96.745046},
				{"latitude": 32.987919, "longitude": -96.746194},
				{"latitude": 32.987982, "longitude": -96.746966},
				{"latitude": 32.987361, "longitude": -96.747020},
				{"latitude": 32.987487, "longitude": -96.746172},
				{"latitude": 32.984653, "longitude": -96.745990},
				{"latitude": 32.984635, "longitude": -96.745367},
				{"latitude": 32.985616, "longitude": -96.745325},
				{"latitude": 32.985616, "longitude": -96.745958},
				{"latitude": 32.987829, "longitude": -96.746204},
				{"latitude": 32.988405, "longitude": -96.744938},			
				{"latitude": 32.988747, "longitude": -96.745046},			
				{"latitude": 32.989359, "longitude": -96.743908},		
				{"latitude": 32.990034, "longitude": -96.744273}			
			]
		},
		{
			"name": "Commons",
			"color": '#9978c8',
			"path": [
				{"latitude":32.985645, "longitude": -96.74914},		
				{"latitude":32.985666, "longitude": -96.750987},
				{"latitude":32.990643, "longitude": -96.750946},
				{"latitude":32.990721, "longitude": -96.753669},
				{"latitude":32.991789, "longitude": -96.753631},
				{"latitude":32.991789, "longitude": -96.752607},
				{"latitude":32.990734, "longitude": -96.752620},
				{"latitude":32.990721, "longitude": -96.750949},
				{"latitude":32.990643, "longitude": -96.750946}				
			]
		},
		{
			"name": "Phase 1 North",
			"color": '#FF9900',
			"path": [
				{"latitude":32.985645, "longitude":-96.74914},			
				{"latitude":32.985668, "longitude":-96.754610},
				{"latitude":32.985850, "longitude":-96.754610},
				{"latitude":32.985864, "longitude":-96.753907},
				{"latitude":32.986955, "longitude":-96.753751},
				{"latitude":32.987130, "longitude":-96.753778},
				{"latitude":32.987124, "longitude":-96.753910},
				{"latitude":32.988195, "longitude":-96.753947},
				{"latitude":32.988219, "longitude":-96.755001},
				{"latitude":32.988388, "longitude":-96.755001},
				{"latitude":32.988368, "longitude":-96.753725},
				{"latitude":32.985668, "longitude":-96.753741}			
			]
		},
		{
			"name": "Phase 1 South",
			"color": '#FF33FF',
			"path": [
				{"latitude":32.985647, "longitude":-96.74914},			
				{"latitude":32.985672, "longitude":-96.754277},
				{"latitude":32.985456, "longitude":-96.754309},
				{"latitude":32.985035, "longitude":-96.753988},
				{"latitude":32.983748, "longitude":-96.754399},
				{"latitude":32.983750, "longitude":-96.755866},
				{"latitude":32.985345, "longitude":-96.755054},
				{"latitude":32.985664, "longitude":-96.755254},
				{"latitude":32.985672, "longitude":-96.754277}			
			]
		},
		{
			"name": "Rutford Rd",
			"color": '#005710',
			"path": [
				{"latitude":32.981273, "longitude":-96.750881},
				{"latitude":32.981273, "longitude":-96.750190},
				{"latitude":32.980547, "longitude":-96.750190},
				{"latitude":32.980504, "longitude":-96.750881},
				{"latitude":32.98422, "longitude":-96.751053},
				{"latitude":32.992023, "longitude":-96.750967},
				{"latitude":32.992068, "longitude":-96.752984},
				{"latitude":32.992768, "longitude":-96.753057},
				{"latitude":32.992757, "longitude":-96.751475},
				{"latitude":32.992068, "longitude":-96.751461}	
			]
		}
	];

		
	for (i = 0; i < routes.length; i++) {
		setRouteFromJSON(routes[i]);
	}
	
  }
  function setRouteFromJSON(route) {
	var routePath = [];
	for (j = 0; j < route.path.length; j++) {
		routePath.push(new google.maps.LatLng(route.path[j].latitude, route.path[j].longitude));
	}
	var routeLine = new google.maps.Polyline({
		path: routePath,
		geodesic: true,
		strokeColor: route.color,
		strokeOpacity: 1.0,
		strokeWeight: 2.5,
		map: map
	});
	
  }


  function setCabColor(isFull, onDuty) {
	var color = "#008000" ;
	if (isFull) {
		color = "#FFFF00";
	}
	if (!onDuty) {
		color = "#FF0000";
	}
	return color;
  }
  
  function setCabMarkers() {
  		/*Code to add a marker representing a single icon to represent a cab.
		The icon does not resize, so I'll have to add some sort of zoom listener
		Will probably use http://stackoverflow.com/questions/3281524/resize-markers-depending-on-zoom-google-maps-v3
		as a reference for that*/
	
	/* The JSON for these should be:
		var jsonCab = {
			"latitude": <double>
			"longitude": <double>
			"isFull": <true or false>
			"onDuty": <true or false>		
		}
		
	*/
	var cabs = [
		{
			"latitude": 32.986498,
			"longitude": -96.751010,
			"isFull": false,
			"onDuty": true
		},
		{
			"latitude": 32.985672,
			"longitude": -96.754399,
			"isFull": false,
			"onDuty": false
		}	
	
	];
	
	for (i = 0; i < cabs.length; i++) {
		drawCab(cabs[i].latitude, cabs[i].longitude, cabs[i].isFull, cabs[i].onDuty);
	}

  }
  
  function drawCab(latitude, longitude, isFull, onDuty) {
	cab = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(latitude, longitude),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: setCabColor(isFull, onDuty),
		strokeOpacity: 1.0,
		strokeColor: setCabColor(isFull, onDuty),
		strokeWeight: 1.0,				
		scale: 7 //pixels
		}		
		});   
  }

  function toggleFull() {
	full = !full;
	if (full) {
		// fullUI.style.backgroundColor='#FFFF00';
		fullUI.style.background = 'rgb(255,255,0)'; /* Old browser support */
		fullUI.style.background = 'rgba(255,255,0,0.5)';
	} else {
		// fullUI.style.backgroundColor='#008000';	
		fullUI.style.background = 'rgb(0,128,0)'; /* Old browser support */
		fullUI.style.background = 'rgba(0,128,0,0.5)';
	}
  
  }
  function FullControl(controlDiv, map) {

	  // Set CSS styles for the DIV containing the control
	  // Setting padding to 5 px will offset the control
	  // from the edge of the map.
	  controlDiv.style.padding = '5px';

	  // Set CSS for the control border.
	  fullUI = document.createElement('div');
	  // fullUI.style.backgroundColor = '#008000';
	  fullUI.style.background = 'rgb(0,128,0)'; /* Old browser support */
	  fullUI.style.background = 'rgba(0,128,0, 0.5)';
	  fullUI.style.borderStyle = 'solid';
	  fullUI.style.borderWidth = '2px';
	  fullUI.style.cursor = 'pointer';
	  fullUI.style.textAlign = 'center';
	  fullUI.title = 'Click to set the map to Home';
	  controlDiv.appendChild(fullUI);

	  // Set CSS for the control interior.
	  var controlText = document.createElement('div');
	  controlText.style.fontFamily = 'Arial,sans-serif';
	  controlText.style.fontSize = '12px';
	  controlText.style.paddingLeft = '4px';
	  controlText.style.paddingRight = '4px';
	  controlText.innerHTML = '<strong>Full Cab Toggle</strong>';
	  fullUI.appendChild(controlText);

	  // Setup the click event listeners: simply set the map to Chicago.
	  google.maps.event.addDomListener(fullUI, 'click', function() {
		toggleFull();
		});
	}
	
	function incrementRiders() {
		totalRiders++;	
		totalText.innerHTML = '<strong>Passengers: ' + totalRiders + '</strong>';
	}
	function decrementRiders() {
		if (totalRiders > 0) {
			totalRiders--;	
		}
		totalText.innerHTML = '<strong>Passengers: ' + totalRiders + '</strong>';
	}
	function IncrementRiderControl(controlDiv, map) {

	  // Set CSS styles for the DIV containing the control
	  // Setting padding to 5 px will offset the control
	  // from the edge of the map.
	  controlDiv.style.padding = '5px';

	  // Set CSS for the control border.
	  var incrementUI = document.createElement('div');
	  // incrementUI.style.backgroundColor = 'white';
	  incrementUI.style.background = 'rgb(255,255,255)'; /* Old browser support */
	  incrementUI.style.background = 'rgba(255,255,255, 0.5)';
	  incrementUI.style.borderStyle = 'solid';
	  incrementUI.style.borderWidth = '2px';
	  incrementUI.style.cursor = 'pointer';
	  incrementUI.style.textAlign = 'center';
	  incrementUI.title = 'Click to set the map to Home';
	  controlDiv.appendChild(incrementUI);

	  // Set CSS for the control interior.
	  var controlText = document.createElement('div');
	  controlText.style.fontFamily = 'Arial,sans-serif';
	  controlText.style.fontSize = '24px';
	  controlText.style.paddingLeft = '4px';
	  controlText.style.paddingRight = '4px';
	  controlText.style.paddingTop = '4px';
	  controlText.style.paddingBottom = '4px';
	  controlText.innerHTML = '<strong>+Rider</strong>';
	  incrementUI.appendChild(controlText);

	  // Setup the click event listeners: simply set the map to Chicago.

	google.maps.event.addDomListener(incrementUI, 'touchstart', function() {
		incrementUI.style.backgroundColor = '#FFFF00';
		});
	google.maps.event.addDomListener(incrementUI, 'touchend', function() {
		// incrementUI.style.backgroundColor = 'white';
		incrementUI.style.background = 'rgb(255,255,255)'; /* Old browser support */
		incrementUI.style.background = 'rgba(255,255,255, 0.5)';
		incrementRiders();
		});

	}
	function DecrementRiderControl(controlDiv, map) {

	  // Set CSS styles for the DIV containing the control
	  // Setting padding to 5 px will offset the control
	  // from the edge of the map.
	  controlDiv.style.padding = '5px';

	  // Set CSS for the control border.
	  var incrementUI = document.createElement('div');
	  // incrementUI.style.backgroundColor = 'white';
	  incrementUI.style.background = 'rgb(255,255,255)'; /* Old browser support */
	  incrementUI.style.background = 'rgba(255,255,255, 0.5)';
	  incrementUI.style.borderStyle = 'solid';
	  incrementUI.style.borderWidth = '2px';
	  incrementUI.style.cursor = 'pointer';
	  incrementUI.style.textAlign = 'center';
	  incrementUI.title = 'Click to set the map to Home';
	  controlDiv.appendChild(incrementUI);

	  // Set CSS for the control interior.
	  var controlText = document.createElement('div');
	  controlText.style.fontFamily = 'Arial,sans-serif';
	  controlText.style.fontSize = '24px';
	  controlText.style.paddingLeft = '4px';
	  controlText.style.paddingRight = '4px';
	  controlText.style.paddingTop = '4px';
	  controlText.style.paddingBottom = '4px';
	  controlText.innerHTML = '<strong>-Rider</strong>';
	  incrementUI.appendChild(controlText);

	  // Setup the click event listeners: simply set the map to Chicago.

	google.maps.event.addDomListener(incrementUI, 'touchstart', function() {
		incrementUI.style.backgroundColor = '#FFFF00';
		});
		google.maps.event.addDomListener(incrementUI, 'touchend', function() {
		// incrementUI.style.backgroundColor = 'white';
		incrementUI.style.background = 'rgb(255,255,255)'; /* Old browser support */
		incrementUI.style.background = 'rgba(255,255,255, 0.5)';
		decrementRiders();
		});
	}
	
	function RiderTotalsView(controlDiv, map) {

	  // Set CSS styles for the DIV containing the control
	  // Setting padding to 5 px will offset the control
	  // from the edge of the map.
	  controlDiv.style.padding = '5px';

	  // Set CSS for the control border.
	  var incrementUI = document.createElement('div');
	  // incrementUI.style.backgroundColor = 'white';
	  incrementUI.style.background = 'rgb(255,255,255)'; /* Old browser support */
	  incrementUI.style.background = 'rgba(255,254,255, 0.5)';
	  incrementUI.style.borderStyle = 'solid';
	  incrementUI.style.borderWidth = '2px';
	  incrementUI.style.cursor = 'pointer';
	  incrementUI.style.textAlign = 'center';
	  incrementUI.title = 'Click to set the map to Home';
	  controlDiv.appendChild(incrementUI);

	  // Set CSS for the control interior.
	  totalText = document.createElement('div');
	  totalText.style.fontFamily = 'Arial,sans-serif';
	  totalText.style.fontSize = '12px';
	  totalText.style.paddingLeft = '4px';
	  totalText.style.paddingRight = '4px';
	  totalText.innerHTML = '<strong>Passengers: ' + totalRiders + '</strong>';
	  incrementUI.appendChild(totalText);

	  // Setup the click event listeners: simply set the map to Chicago.

	}
	  /*Last line of code from my things. For Driver, you just initialize() rather than doing window.onLoad*/
	  initialize();
      //google.maps.event.addDomListener(window, 'load', initialize);
	/*Everything after here was from example code (http://paulsutherland.net/ionic-and-google-maps-api/. 
		I don't know how these affect the app*/
      $scope.centerOnMe = function() {
        if(!$scope.map) {
          return;
        }

        $scope.loading = $ionicLoading.show({
          content: 'Getting current location...',
          showBackdrop: false
        });
        navigator.geolocation.getCurrentPosition(function(pos) {
          $scope.map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
          $scope.loading.hide();
        }, function(error) {
          alert('Unable to get location: ' + error.message);
        });
      };
      
      $scope.clickTest = function() {
        alert('Example of infowindow with ng-click')
      };
      
   });