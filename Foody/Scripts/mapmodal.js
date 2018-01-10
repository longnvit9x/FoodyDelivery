var var_map;
var var_location = new google.maps.LatLng(45.430817, 12.331516);
function map_init() {

    var mapoptions = {
        center: var_location,
        zoom: 18,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        mapTypeControl: false,
        panControl: false,
        rotateControl: false,
        streetViewControl: false,
    };
    var_map = new google.maps.Map(document.getElementById("map"),
        mapoptions);
   
    var var_contentString =
         '<div id="mapInfo">' +
         '<p><strong>Peggy Guggenheim Collection</strong><br><br>' +
         'Dorsoduro, 701-704<br>' +
         '30123<br>Venezia<br>' +
         'P: (+39) 041 240 5411</p>' +
         '<a href="http://www.guggenheim.org/venice" target="_blank">Plan your visit</a>' +
         '</div>';

    var infowindow = new google.maps.InfoWindow({
        content: var_contentString
    });
    var infowindowContent = document.getElementById('infowindow-content');
    infowindow.setContent(infowindowContent);

    var marker = new google.maps.Marker({
        position: var_location,
        map: var_map,
        title: "Click for information about the Guggenheim museum in Venice",
        maxWidth: 200,
        maxHeight: 200
    });
   
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(var_map, marker);
    });

    var input = document.getElementById('pac-input');

    var autocomplete = new google.maps.places.Autocomplete(
        input, {placeIdOnly: true});
    autocomplete.bindTo('bounds', var_map);

    var_map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // geo
    var geocoder = new google.maps.Geocoder;
    autocomplete.addListener('place_changed', function() {
        infowindow.close();
        debugger;
        var place = autocomplete.getPlace();

        if (!place.place_id) {
            return;
        }
        geocoder.geocode({'placeId': place.place_id}, function(results, status) {
            
            if (status !== 'OK') {
                window.alert('Geocoder failed due to: ' + status);
                return;
            }
            var_map.setZoom(18);
            var_map.setCenter(results[0].geometry.location);
            // Set the position of the marker using the place ID and location.
            marker.setPlace({
                placeId: place.place_id,
                location: results[0].geometry.location
            });
            marker.setVisible(true);
            infowindowContent.children['place-name'].textContent = place.name;
            infowindowContent.children['place-id'].textContent = place.place_id;
            infowindowContent.children['place-address'].textContent =
                results[0].formatted_address;
            infowindow.open(var_map, marker);
        });
    });
}

google.maps.event.addDomListener(window, 'load', map_init);

//start of modal google map
$('#mapmodals').on('shown.bs.modal', function () {
    google.maps.event.trigger(var_map, "resize");
    var_map.setCenter(var_location);
});
