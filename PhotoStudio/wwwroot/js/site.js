// Write your JavaScript code.
var weekShiftDelta = 0;
var elementsArray = document.getElementsByClassName('daycell');


function getNumber(index){
    var dayPosition = weekShiftDelta * 7  + index;
    var request = new XMLHttpRequest();
    request.open("POST", SelectedDayURL, false);
    request.send(dayPosition);
}


Array.from(elementsArray).forEach(
    function(elem, index) {
        elem.addEventListener('click', function() { getNumber(index); });
    });


function calendar_down(url) {
    var request = new XMLHttpRequest();
    request.open("POST", url, false);
    request.send(++weekShiftDelta);
    var r = JSON.parse(request.response);
    calenadrUpdate(r);    
}

function calendar_up(url) {
    var request = new XMLHttpRequest();
    request.open("POST", url, false);
    if (weekShiftDelta>0){
        request.send(--weekShiftDelta);
        var r = JSON.parse(request.response);
        calenadrUpdate(r);  
    }
          
}

function calenadrUpdate(json) {
    var obj = document.getElementsByClassName("numbercell");
    for(var i = 0; i<obj.length; i++){        
        obj[i].firstChild.nodeValue = json.value.calendarData[i].date;
        obj[i].firstChild.nextElementSibling.innerHTML = json.value.calendarData[i].month;
    }
}