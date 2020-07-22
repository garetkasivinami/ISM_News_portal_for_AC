﻿let currentDate = new Date();

let dates = document.querySelectorAll(".js_date");
for (let i = 0; i < dates.length; i++) {
    let dateParse = Date.parse(dates[i].innerHTML);
    let date = new Date(dateParse);
    editTime.innerHTML = date.toLocaleDateString() + " " + date.toLocaleTimeString();
}

let startUpdateCommection = document.querySelectorAll(".submit_date");
for (let i = 0; i < startUpdateCommection.length; i++) {
    let dateParse = Date.parse(startUpdateCommection[i].value);
    if (dateParse == NaN) {
        continue;
    }
    let date = new Date(dateParse);
    date.setMinutes(date.getMinutes() - currentDate.getTimezoneOffset() * 2);
    startUpdateCommection[i].value = date.toISOString().slice(0, 16);
}

var tz = currentDate.getTimezoneOffset(); 
let submitDateCollection = document.querySelectorAll(".submit_date_offset");
for (let i = 0; i < submitDateCollection.length; i++) {
    submitDateCollection[i].value = tz;
}