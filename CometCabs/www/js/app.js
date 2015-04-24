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
    var interestId;
	var map;
    var status;
	function initialize() {
		/*Latitude and longitude for the school. Don't know whether we could use more precision */
        var site = new google.maps.LatLng(32.986,-96.750);
		
      
        var mapOptions = {
          streetViewControl:true,
          center: site,
          zoom: 15, /*This can be played with. It's either +-1 that we can see building shapes */
        };
		/*The element references the div id in index.html*/
        map = new google.maps.Map(document.getElementById("map"),
            mapOptions);
        
        //setRouteCoordinates(); //Sets the details for routes
		//setCabMarkers();
        setRouteChoice();
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
    
    function setRouteChoice() {
      //load dropdown route choice
      var url = 'http://cometcabs.azurewebsites.net/api/login';
 
            var xhr = createCORSRequest('GET', url);
 
            if (!xhr) {
                alert('CORS not supported');
                return;
            }
 
            // Response handlers.
            xhr.onload = function () {
                var resources = JSON.parse(xhr.responseText);
                var routes = resources.Routes;
                routes.push({"RouteId":0,"RouteName":"All"});
				
				var firstLoad = true;
				if($scope.allRoutes) {
					firstLoad = false;
				}
				
                $scope.allRoutes = routes;
				
				var listItems= "";
				for (var i = 0; i < $scope.allRoutes.length; i++){
					listItems+= "<option value='" + $scope.allRoutes[i].RouteId + "'>" + $scope.allRoutes[i].RouteName + "</option>";
				}
				var si = document.getElementById("route").selectedIndex;
				$("#route").html(listItems);
				if(!firstLoad && si != 0 && si > 0) {
					document.getElementById("route").selectedIndex = si;
				} else if(firstLoad) {
					document.getElementById("route").selectedIndex = $scope.allRoutes.length - 1;
				}
            };
 
            xhr.onerror = function () {
                alert('Error making the request.');
            };
 
            xhr.send();
    }
  
    function setRouteCoordinates() {
        //get rider route viewing choice from drop down
        var routeIDDropDown = document.getElementById("route");
        var routeName = routeIDDropDown.options[routeIDDropDown.selectedIndex].text;
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
                var JSONroutes = JSON.parse(xhr.responseText);
                //alert("'" + routeName + "'");
                if (routeName != 'All' && routeName) {
                    for (i = 0; i < JSONroutes.length; i++) {
                        if (routeName == JSONroutes[i].Name) {
                            setRouteFromJSON(JSONroutes[i]);
                        }
                    }
                    removeOldRoutes(); 
                } else if (!routeName || routeName == 'All') {
                    for (i = 0; i < JSONroutes.length; i++) {
                        setRouteFromJSON(JSONroutes[i]);
                    }
                    removeOldRoutes();                    
                }
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
                //alert(cabs[0].MaxCapacity);
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
        var iconOpen = 'img/busgreen.png';
        var iconFull = 'img/busred.png';
        var icon = iconOpen;
          var currentCapacity = cab.Capacity;
          var maxCapacity = cab.MaxCapacity;
          var status = cab.CurrentStatus;
          var fullVal = maxCapacity - currentCapacity;
          if (fullVal <= 0){
              //status = "full";
              icon = iconFull;
          }
          cabMarker = new google.maps.Marker({
              map: map,
              position: new google.maps.LatLng(cab.Latitude, cab.Longitude),
              icon: icon
              /*icon: {
                  path: google.maps.SymbolPath.CIRCLE,
                  fillOpacity: 1.0,
                  fillColor: setCabColor(status),
                  strokeOpacity: 1.0,
                  strokeColor: setCabColor(status),
                  scale: 7, //pixels
                  strokeWeight: 1.0
              }*/
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
    
    function refresh() {
        setRouteChoice();
        setRouteCoordinates(); //Sets the details for routes
		setCabMarkers();
    }
  
	$scope.iWantToRide = function() {
		var btn = document.getElementById("iWantToRideButton");
		var activeColor = 'green';
		
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
                    interestId = id.interestId;
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
                var url = 'http://cometcabs.azurewebsites.net/api/CancelInterest?interestId=' + interestId;

                var xhr = createCORSRequest('POST', url);

                if (!xhr) {
                    alert('CORS not supported');
                    return;
                }

                // Response handlers.
                xhr.onload = function () {
                    var response = JSON.parse(xhr.responseText);
                    //alert(response);
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
