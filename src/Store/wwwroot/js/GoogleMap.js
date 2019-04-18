var map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 49.8397, lng: 24.0297 },
        zoom: 8
    });
}

function setMarker() {
    var select = document.getElementById('EndPointCity');
    var city = select.options[select.selectedIndex].value;

    var adress = city + ", " +
        document.getElementById('EndPointStreet').value;
    var resultlat = ''; var resultlng = '';
    $.ajax({
        async: false,
        dataType: "json",
        url: 'https://maps.google.com/maps/api/geocode/json?address=' + adress + '&key=AIzaSyCFlwlczCmamaKRfISTv2XvFJNttALOfnI',
        success: function (data) {
            for (var key in data.results) {
                resultlat = data.results[key].geometry.location.lat;
                resultlng = data.results[key].geometry.location.lng;
            }
        }
    });

    var marker = new google.maps.Marker({ position: { lat: resultlat, lng: resultlng }, map: map });
    map.panTo(new google.maps.LatLng(resultlat, resultlng));
    setDeliveryPrice();
}

function setDeliveryPrice() {
    var storages = document.getElementById('avaliableStorages');
    var additonalPrice = 120;

    if (storages.length == 0) {
        document.getElementById('deliveryPrice').textContent = additonalPrice;
        document.getElementById('commonPrice').textContent = '@Model.CommonPrice';
        var price = Number(document.getElementById('commonPrice').value);
        document.getElementById('commonPrice').value = (price + additonalPrice).toString();

        return;
    }

    var select = document.getElementById('EndPointCity');
    var city = select.options[select.selectedIndex].value;
    var isTheSamePlace = false;

    for (var i = 0; i < storages.length; i++) {
        if (storages[i].value == city) {
            document.getElementById('deliveryPrice').textContent = 15;
            document.getElementById('commonPrice').value = '@Model.CommonPrice';
            var price = Number(document.getElementById('commonPrice').value);
            document.getElementById('commonPrice').value = (price + 15).toString();
            isTheSamePlace = true;
        }
    }

    if (!isTheSamePlace) {
        document.getElementById('deliveryPrice').textContent = additonalPrice;
        document.getElementById('commonPrice').value = '@Model.CommonPrice';
        var price = Number(document.getElementById('commonPrice').value);
        document.getElementById('commonPrice').value = (price + additonalPrice).toString();
    }
}