// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('starter', ['ionic'])

.controller('MapCtrl', function($scope, $ionicLoading, $compile) {
	var rutfordCoords;
	var phase1SouthCoords;
	var phase1NorthCoords;
	var commonsCoords;
	var mcDermottCoords;
	
	var rutfordRoute;		
	var phase1SouthRoute;	
	var phase1NorthRoute;
	var commonsRoute;	
	var mcDermottRoute;
		
	var rutfordMarker; 
	var phase1SouthMarker;
	var cabs = [];
	var routes = [];
	var newcabs = [];
	var newroutes = [];
	var newriders = [];
	var map;
	function initialize() {
		/*Latitude and longitude for the school. Don't know whether we could use more precision */
        var site = new google.maps.LatLng(32.986,-96.750);
		
      
        var mapOptions = {
          streetViewControl:true,
          center: site,
          zoom: 16, /*This can be played with. It's either +-1 that we can see building shapes */
        };
		/*The element references the div id in index.html*/
        map = new google.maps.Map(document.getElementById("map"),
            mapOptions);
        
        //setRouteCoordinates(); //Sets the details for routes
		//setCabMarkers();
		setInterval(refresh, 1000);
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
		
		google.maps.event.addListener(map, 'zoom_changed', function() {
     if (map.getZoom() < 16) map.setZoom(16);
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
  
    function setRouteFromJSON(route, color) {
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
  
  function drawCab(cab) {
      var currentCapacity = cab.Capacity;
      var maxCapacity = 5; //change this to cab.MaxCapacity
      var status = cab.CurrentStatus;
      if (currentCapacity == maxCapacity){
          status = "full";
      }
	cab = new google.maps.Marker({
		map: map,
		position: new google.maps.LatLng(cab.Latitude, cab.Longitude),
		icon: {
		path: google.maps.SymbolPath.BACKWARD_CLOSED_ARROW,
		fillOpacity: 1.0,
		fillColor: setCabColor(status),
		strokeOpacity: 1.0,
		strokeColor: setCabColor(status),
		strokeWeight: 1.0,				
		scale: 7 //pixels
		}		
		});  
      cabs.push(cab);
  }
    
    /*function removeCabs(){
        for (i = 0; i< cabs.length; i++){
            cabs[i].setMap(null);
        }
        cabs = [];
    }*/
    
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
    
    function refresh() {
        setRouteCoordinates(); //Sets the details for routes
		setCabMarkers();
    }
  
	$scope.iWantToRide = function() {
		var btn = document.getElementById("iWantToRideButton");
		var activeColor = 'green';
        var interestId = '';
		
		if(btn.style.color != activeColor) {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function(position) {
                var latitude = position.coords.latitude;
                var longitude = position.coords.longitude;
                var url = 'http://cometcabs.azurewebsites.net/api/Interests?longitude='+longitude+'&latitude='+latitude;

                var xhr = createCORSRequest('POST', url);

                if (!xhr) {
                    alert('CORS not supported');
                    return;
                }

                // Response handlers.
                xhr.onload = function () {
                    var id = JSON.parse(xhr.responseText);
                    alert(id);
                    interestId = id.interestId;
                    //this is telling me undefined even though xhr.responseText is giving an id back
                    alert(interestId);
                };

                xhr.onerror = function () {
                    alert('Error making the request.');
                };

                xhr.send();
                });
            } else {
                alert("Geolocation is not supported by this browser.");
            }
			btn.style.color = activeColor;
		} else {
                var url = 'http://cometcabs.utd.edu/api/CancelInterest?interestId=' + interestId;

                var xhr = createCORSRequest('POST', url);

                if (!xhr) {
                    alert('CORS not supported');
                    return;
                }

                // Response handlers.
                xhr.onload = function () {
                    var response = JSON.parse(xhr.responseText);
                    alert(response);
                };

                xhr.onerror = function () {
                    alert('Error making the request.');
                };

                xhr.send();

			btn.style.color = 'white';
		}
	};

	  /*Last line of code from my things*/
      google.maps.event.addDomListener(window, 'load', initialize);
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
