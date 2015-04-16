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

.controller('LoginCtrl', function($scope, LoginService, $ionicPopup, $state, $http) {
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
	
	$http.get('routeData.json').success(function(data) {
		$scope.allRoutes = data;
		$scope.RouteIDSelect = "1";
	});
	
	$http.get('cabData.json').success(function(data) {
		$scope.allCabs = data;
		$scope.CabIDSelect = "1";
	});
})

.controller('MapCtrl', function($scope, $ionicLoading, $compile) {
			
	var rutfordMarker; 
	var phase1SouthMarker;
	var chicago = new google.maps.LatLng(41.850033, -87.6500523);
	var map;
    var cabs = [];
	var routes = [];
	var riders = []; //Note, driver app only. If copying things to the rider app, leave this out.
	var newcabs = [];
	var newroutes = [];
	var newriders = [];
	var full;
	var fullControl;
	var fullUI;	
	var totalRiders;
	var totalText;
	function initialize() {
		/*Latitude and longitude for the school. Don't know whether we could use more precision */
        var site = new google.maps.LatLng(32.986,-96.750);
        
        //var site = new google.maps.LatLng(33.135307, -96.737604);
		
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
        updateGPSLocation();
        //setRouteCoordinates(); //Sets the details for routes
		//setCabMarkers();
		setInterval(refresh, 1000);
        /*Sets a marker for the current position */
		/*if (navigator.geolocation) {
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
		}*/
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
    
  function createCORSRequest(method, url) {
        var xhr = new XMLHttpRequest();
        if ("withCredentials" in xhr) {
            // XHR for Chrome/Firefox/Opera/Safari.
            xhr.open(method, url, true);
        } else if (typeof XDomainRequest != "undefined") {
            // XDomainRequest for IE.
            xhr = new XDomainRequest();
            xhr.open(method, url);
        } else {
            // CORS not supported.
            xhr = null;
        }
        return xhr;    
    }
    
    function updateGPSLocation() {
        /*Sets a marker for the current position */
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(function(position) {
                var latitude = position.coords.latitude;
                var longitude = position.coords.longitude;
			//var currentPosition = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
			var url = 'http://cometcabs.azurewebsites.net/api/CabActivity?activityId=4&currentCapacity=2&currentStatus=On%20Duty&latitude=' + latitude + '&longitude=' + longitude;
 
            var xhr = createCORSRequest('POST', url);
 
            if (!xhr) {
                alert('CORS not supported');
                return;
            }
 
            // Response handlers.
            xhr.onload = function () {
                //alert(xhr.responseText);
                
            };
 
            xhr.onerror = function () {
                alert('Error making the request.');
            };
 
            xhr.send();		
			});
		} else {
			alert("Geolocation is not supported by this browser.");
		}
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
            var url = 'http://cometcabs.azurewebsites.net/api/routes';
 
            var xhr = createCORSRequest('GET', url);
 
            if (!xhr) {
                alert('CORS not supported');
                return;
            }
 
            // Response handlers.
            xhr.onload = function () {
                var routes = JSON.parse(xhr.responseText);
                for (i = 0; i < routes.length; i++) {
                    setRouteFromJSON(routes[i]);
                }
                removeOldRoutes();
            };
 
            xhr.onerror = function () {
                alert('Error making the request.');
            };
 
            xhr.send();
	
  }
  
    function setRouteFromJSON(route) {
        var routePath = [];
        for (j = 0; j < route.Path.length; j++) {
            routePath.push(new google.maps.LatLng(route.Path[j].Latitude, route.Path[j].Longitude));
        }
        var routeLine = new google.maps.Polyline({
            path: routePath,
            geodesic: true,
            strokeColor: route.Color,
            strokeOpacity: 1.0,
            strokeWeight: 2.5,
            map: map
        });
		newroutes.push(routeLine);
	}

	function removeOldRoutes() {
		for (i = 0; i < routes.length; i++) {
			routes[i].setMap(null);
		}
		routes = [];
		for (i = 0; i < newroutes.length; i++) {
			routes.push(newroutes[i]);
		}
		newroutes = [];
	}
  function setCabColor(status) {
	var color = "#008000" ; // == "onduty"
	if (status == "full") {
		color = "#FFFF00";
	}
	if (status == "offduty") {
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
			"CabCode": Cab#
			"RouteName": Name of Route
			"Capacity": number
			"CurrentStatus": full, onduty, offduty
            "Longitude": <double>
            "Latitude": <double>
		}
		
	*/
	       var url = 'http://cometcabs.azurewebsites.net/api/CabActivity';
 
            var xhr = createCORSRequest('GET', url);
 
            if (!xhr) {
                alert('CORS not supported');
                return;
            }
 
            // Response handlers.
            xhr.onload = function () {
                var cabs = JSON.parse(xhr.responseText);
                for (i = 0; i < cabs.length; i++) {
                    drawCab(cabs[i]);
                }
                removeOldCabs();
            };
 
            xhr.onerror = function () {
                alert('Error making the request.');
            };
 
            xhr.send();

  }
  
    function setRiderMarkers() {
  		/*Code to add a marker representing a single icon to represent a cab.
		The icon does not resize, so I'll have to add some sort of zoom listener
		Will probably use http://stackoverflow.com/questions/3281524/resize-markers-depending-on-zoom-google-maps-v3
		as a reference for that*/
	
	/* The JSON for these should be:
		var jsonRider = {			
            "Longitude": <double>
            "Latitude": <double>
		}
		
	*/
	       var url = 'http://cometcabs.azurewebsites.net/api/RiderActivity';
 
            var xhr = createCORSRequest('GET', url);
 
            if (!xhr) {
                alert('CORS not supported');
                return;
            }
 
            // Response handlers.
            xhr.onload = function () {
                var cabs = JSON.parse(xhr.responseText);
                for (i = 0; i < cabs.length; i++) {
                    drawRider(riders[i]);
                }				
                removeOldRiders();
            };
 
            xhr.onerror = function () {
                alert('Error making the request.');
            };
 
            xhr.send();

  }
  
    function drawRider(rider) {
		riderMarker = new google.maps.Marker({
			map: map,
			position: new google.maps.LatLng(rider.Latitude, rider.Longitude),
			title: 'Rider Waiting'
			});  
		newriders.push(rider);
	}
	/*Takes all the cabs in the list of already existing riders and removes
	  them from the map. The riders last fetched remain and are copied for
	  removal during the next cycle. */
	function removeOldRiders() {
		for (i = 0; i< riders.length; i++){
            riders[i].setMap(null);
        }
        riders = [];
		for (i = 0; i < newriders.length; i++) {
			riders.push(newriders[i]);
		}
		newriders = [];
	}
     
  function drawCab(cab) {
	cabMarker = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(cab.Latitude, cab.Longitude),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: setCabColor(cab.Status),
		strokeOpacity: 1.0,
		strokeColor: setCabColor(cab.Status),
		strokeWeight: 1.0,				
		scale: 7 //pixels
		}		
		});  
      newcabs.push(cabMarker);
  }
    
    function removeOldCabs(){
        for (i = 0; i< cabs.length; i++){
            cabs[i].setMap(null);
        }
        cabs = [];
		for (i = 0; i < newcabs.length; i++) {
			cabs.push(newcabs[i]);
		}
		newcabs = [];
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
    function refresh() {
        setRouteCoordinates(); //Sets the details for routes        
		setCabMarkers();
		//setRiderMarkers():
        updateGPSLocation();
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