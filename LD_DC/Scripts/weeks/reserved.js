//var filterBoxEl = document.getElementById('filter-box');
//var filterContentEl = filterBoxEl.children[0];
//var reservedBoxEl = document.getElementById('reserved-box');
//var reservedContentEl = reservedBoxEl.children[0];
var selectDate = document.getElementById('select-date');


var selectDay = {};

var filterBox = new Vue({
    el: '#filter-box',
    data: {
        isShow: false,
        people: null,
        start: null,
        end: null,
        hardware: null,
        filterAll: true,
        filterPlace: false,
        filterLocation: false,
        devices: [{
            active: false,
            name: "投影仪"
        },
        {
            active: false,
            name: "白板"
        },
        {
            active: false,
            name: "视频"
        },
        {
            active: false,
            name: "电话"
        },
        {
            active: false,
            name: "显示器"
        }
        ],
        hours: [{
            active: false,
            hour: "9-10",
            start: "09",
            end: "10"
        },
        {
            active: false,
            hour: "11-12",
            start: "11",
            end: "12"
        },
        {
            active: false,
            hour: "13-14",
            start: "13",
            end: "14"
        },
        {
            active: false,
            hour: "15-16",
            start: "15",
            end: "16"
        },
        {
            active: false,
            hour: "17-19",
            start: "17",
            end: "19"
        }
        ],
        moments: [{
            active: false,
            moment: "上午",
            start: "09",
            end: "12"
        },
        {
            active: false,
            moment: "下午",
            start: "12",
            end: "19"
        },
        {
            active: false,
            moment: "全天",
            start: "09",
            end: "19"
        }
        ],
        peoples: [{
            active: false,
            people: 4,
            peopleText: "4人"
        },
        {
            active: false,
            people: 8,
            peopleText: "8人"
        },
        {
            active: false,
            people: 10,
            peopleText: "10人"
        },
        {
            active: false,
            people: 20,
            peopleText: "20人"
        },
        {
            active: false,
            people: 50,
            peopleText: "50＋"
        }
        ],
        places: [],
        placeText: null,
        locations: [],
        locationText: null
    },
    ready: function () {
        this.getPlace();
    },
    methods: {
        offFilter: function (msg, event) {
            removeClass(filterContentEl, "slideInUp");
            addClass(filterContentEl, "slideOutDown");
            setTimeout(function () {
                filterContentEl.style.display = "none";
                roomList.stop = false;
                filterBox.filterAll = true;
                filterBox.filterPlace = false;
                filterBox.filterLocation = false;
                filterBox.isShow = false;
            }, 300);
        },
        confirmFilter: function (msg, event) {
            removeClass(filterContentEl, "slideInUp");
            addClass(filterContentEl, "slideOutDown");
            roomList.items = [];
            roomList.getRoomList();
            setTimeout(function () {
                roomList.stop = false;
                filterContentEl.style.display = "none";
                filterBox.isShow = false;
            }, 300);
        },
        selectDevice: function (msg, event) {
            event.active = !event.active;
            var devicesT = [];
            for (var i = 0; i < this.devices.length; i++) {
                if (this.devices[i].active) {
                    devicesT.push(this.devices[i].name);
                }
            }
            this.hardware = devicesT.join(",");
        },
        selectHour: function (msg, event) {
            if (event.active) {
                return;
            }
            for (var i = 0; i < this.hours.length; i++) {
                this.hours[i].active = false;
            }
            for (var i = 0; i < this.moments.length; i++) {
                this.moments[i].active = false;
            }
            event.active = !event.active;
            this.start = event.start;
            this.end = event.end;
        },
        selectMoment: function (msg, event) {
            if (event.active) {
                return;
            }
            for (var i = 0; i < this.hours.length; i++) {
                this.hours[i].active = false;
            }
            for (var i = 0; i < this.moments.length; i++) {
                this.moments[i].active = false;
            }
            event.active = !event.active;
            this.start = event.start;
            this.end = event.end;
        },
        selectPeople: function (msg, event) {
            if (event.active) {
                return;
            }
            for (var i = 0; i < this.peoples.length; i++) {
                this.peoples[i].active = false;
            }
            event.active = !event.active;
            this.people = event.people;
        },
        onSelectPlace: function (msg, event) {
            for (var i = 0; i < this.places.length; i++) {
                if (this.placeText == this.places[i].name) {
                    this.places[i].active = true;
                } else {
                    this.places[i].active = false;
                }
            }
            this.filterAll = false;
            this.filterPlace = true;
            this.filterLocation = false;
        },
        onSelectLocation: function (msg, event) {
            for (var i = 0; i < this.locations.length; i++) {
                if (this.locationText == this.locations[i].name) {
                    this.locations[i].active = true;
                } else {
                    this.locations[i].active = false;
                }
            }
            this.filterAll = false;
            this.filterPlace = false;
            this.filterLocation = true;
        },
        selectPlace: function (msg, event) {
            for (var i = 0; i < this.places.length; i++) {
                this.places[i].active = false;
            }
            event.active = true;
        },
        selectLocation: function (msg, event) {
            for (var i = 0; i < this.locations.length; i++) {
                this.locations[i].active = false;
            }
            event.active = true;
        },
        savePlace: function (msg, event) {
            var placeId;
            for (var i = 0; i < this.places.length; i++) {
                if (this.places[i].active) {
                    placeId = this.places[i].id;
                    break;
                }
            }

            if (placeId) {
                showLoading();
                this.$http({
                    method: 'post',
                    url: baseRequestUrl + 't=ddsz_place_updateUserPlace',
                    params: {
                        uid: wxData.userId,
                        placeid: placeId,
                        mainbodyid: wxData.mainBodyId
                    },
                    credientials: true,
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    },
                    emulateJSON: false,
                    timeout: 300000,
                    progress: function (progress) { }
                }).then(function (response) {
                    hideLoading();
                    if (response.ok && validate(response.body)) {
                        var result;
                        if (typeof (response.body) == "object") {
                            result = response.body;
                        } else {
                            result = JSON.parse(response.body);
                        }

                        var data;
                        if (result.data) {
                            toast.onToast("修改地点成功");
                            data = result.data;
                            setCookie('AddressId', placeId);
                            wxData.placeId = placeId;
                            for (var i = 0; i < this.places.length; i++) {
                                if (this.places[i].active) {
                                    this.placeText = this.places[i].name;
                                }
                            }

                            this.offPlace();
                            this.getLocation();
                        } else {
                            toast.onToast(result.msg);
                            return;
                        }
                        console.log(data);
                    }
                }, function (response) {
                    hideLoading();
                    console.log(response);
                }).catch(function (response) {
                    hideLoading();
                    console.log(response);
                });
            }
        },
        offPlace: function () {
            this.filterAll = true;
            this.filterPlace = false;
            this.filterLocation = false;
        },
        saveLocation: function (msg, event) {
            for (var i = 0; i < this.locations.length; i++) {
                if (this.locations[i].active) {
                    roomList.roomId = this.locations[i].name;
                    this.locationText = this.locations[i].name;
                }
            }
            this.offLocation();
        },
        offLocation: function (msg, event) {
            this.filterAll = true;
            this.filterPlace = false;
            this.filterLocation = false;
        },
        getPlace: function () {
            this.$http({
                method: 'post',
                url: baseRequestUrl + 't=ddsz_Module_select',
                params: {
                    mainbodyid: wxData.mainBodyId
                },
                credientials: true,
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                emulateJSON: false,
                timeout: 300000,
                progress: function (progress) { }
            }).then(function (response) {
                hideLoading();
                if (response.ok && validate(response.body)) {
                    var result;
                    if (typeof (response.body) == "object") {
                        result = response.body;
                    } else {
                        result = JSON.parse(response.body);
                    }

                    var data;
                    if (result.data) {
                        data = result.data;
                    } else {
                        toast.onToast(result.msg);
                        return;
                    }

                    for (var i = 0; i < data.length; i++) {
                        var active = false;
                        if (data[i].id == wxData.placeId) {
                            active = true;
                            this.placeText = data[i].title;
                        }
                        this.places.push({
                            active: active,
                            id: data[i].id,
                            name: data[i].title,
                            address: data[i].address
                        });
                    }
                    this.getLocation();
                }
            }, function (response) {
                hideLoading();
                console.log(response);
            }).catch(function (response) {
                hideLoading();
                console.log(response);
            });
        },
        getLocation: function () {
            showLoading();
            this.locations = [];
            this.locationText = null;
            this.$http({
                method: 'post',
                url: baseRequestUrl + 't=hyfw_tyease2_hys',
                params: {
                    placeid: wxData.placeId
                },
                credientials: true,
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                emulateJSON: false,
                timeout: 300000,
                progress: function (progress) { }
            }).then(function (response) {
                hideLoading();
                if (response.ok && validate(response.body)) {
                    var result;
                    if (typeof (response.body) == "object") {
                        result = response.body;
                    } else {
                        result = JSON.parse(response.body);
                    }

                    var data;
                    if (result.data) {
                        data = result.data;
                    } else {
                        toast.onToast(result.msg);
                        return;
                    }
                    console.log(data);

                    for (var i = 0; i < data.length; i++) {
                        this.locations.push({
                            active: false,
                            id: i,
                            name: data[i]
                        });
                    }
                }
            }, function (response) {
                hideLoading();
                console.log(response);
            }).catch(function (response) {
                hideLoading();
                console.log(response);
            });
        },
        resetFilter: function (msg, event) {
            this.people = null;
            this.start = null;
            this.end = null;
            this.hardware = null;
            roomList.roomId = null;
            this.locationText = null;
            for (var i = 0; i < this.hours.length; i++) {
                this.hours[i].active = false;
            }
            for (var i = 0; i < this.moments.length; i++) {
                this.moments[i].active = false;
            }
            for (var i = 0; i < this.devices.length; i++) {
                this.devices[i].active = false;
            }
            for (var i = 0; i < this.peoples.length; i++) {
                this.peoples[i].active = false;
            }
        }
    }
});

var filterButton = new Vue({
    el: '#filter-button',
    methods: {
        onFilter: function (msg, event) {
            for (var i = 0; i < roomList.items.length; i++) {
                roomList.items[i].active = false;
                roomList.items[i].activeClass = "";
            }
            roomList.stop = true;
            filterBox.isShow = true;
            setTimeout(function () {
                filterContentEl.style.display = "block";
            }, 50);
            removeClass(filterContentEl, "slideOutDown");
            addClass(filterContentEl, "slideInUp");
        }
    }
})

var hoursT = [{
    hour: 8,
    text: ""
},
{
    hour: 9,
    text: "9"
},
{
    hour: 10,
    text: ""
},
{
    hour: 11,
    text: ""
},
{
    hour: 12,
    text: "12"
},
{
    hour: 13,
    text: ""
},
{
    hour: 14,
    text: ""
},
{
    hour: 15,
    text: "15"
},
{
    hour: 16,
    text: ""
},
{
    hour: 17,
    text: ""
},
{
    hour: 18,
    text: "18"
},
{
    hour: 19,
    text: ""
}
];

var roomList = new Vue({
    el: '#room-list',
    data: {
        roomId: null,
        stop: false,
        items: [
            // {
            // 	zoom: false,
            // 	zoomClass: "",
            // 	active: false,
            // 	activeClass: "",
            // 	title: "第一会议室",
            // 	hours: [
            // 		{
            // 			hour: 8,
            // 			text: "",
            // 			status: "busy"
            // 		},
            // 		{
            // 			hour: 9,
            // 			text: "9",
            // 			status: "busy"
            // 		},
            // 		{
            // 			hour: 10,
            // 			text: "",
            // 			status: "free"
            // 		},
            // 		{
            // 			hour: 11,
            // 			text: "",
            // 			status: "free"
            // 		},
            // 		{
            // 			hour: 12,
            // 			text: "12",
            // 			status: "already"
            // 		},
            // 		{
            // 			hour: 13,
            // 			text: "",
            // 			status: "already"
            // 		},
            // 		{
            // 			hour: 14,
            // 			text: "",
            // 			status: "already"
            // 		},
            // 		{
            // 			hour: 15,
            // 			text: "15",
            // 			status: "already"
            // 		},
            // 		{
            // 			hour: 16,
            // 			text: "",
            // 			status: "free"
            // 		},
            // 		{
            // 			hour: 17,
            // 			text: "",
            // 			status: "free"
            // 		},
            // 		{
            // 			hour: 18,
            // 			text: "18",
            // 			status: "free"
            // 		},
            // 		{
            // 			hour: 19,
            // 			text: "",
            // 			status: "free"
            // 		}
            // 	],
            // 	devices: {
            // 		projector: true,
            // 		board: true,
            // 		video: true,
            // 		phone: true,
            // 		computer: true
            // 	},
            // 	people: 10
            // }
        ]
    },
    methods: {
        zoomInOut: function (msg, event) {
            event.zoom = !event.zoom;
            event.zoom ? (event.zoomClass = "zoom") : (event.zoomClass = "");
        },
        onReserved: function (msg, event, parent) {
            offScroll();
            var nowItem = this.items[this.items.indexOf(parent)];
            var nowHour = nowItem.hours.indexOf(event);
            if (!nowItem.active) {
                if (nowItem.hours[nowHour].isNetwork) {
                    return;
                }

                event.status = "already";
                nowItem.active = true;
                nowItem.activeClass = "active";
                reservedBox.isShow = true;
                reservedBox.id = parent.id;
                reservedBox.devices = nowItem.devices;
                reservedBox.start = event.hour;
                reservedBox.thumb = nowItem.thumb;
                reservedBox.title = nowItem.title;
                reservedBox.address = nowItem.address;
                for (var i = (nowHour + 1); i < nowItem.hours.length; i++) {
                    if (nowItem.hours[i].isNetwork) {
                        reservedBox.maxEnd = nowItem.hours[i - 1].hour;
                        break;
                    }

                    if (i >= nowItem.hours.length - 1) {
                        reservedBox.maxEnd = nowItem.hours[nowItem.hours.length - 1].hour;
                        break;
                    }
                }
                reservedBox.end = reservedBox.start + 1;
                reservedBox.long = 1;
            } else {
                if (event.hour < reservedBox.start || event.hour > reservedBox.maxEnd) {
                    return;
                }
                reservedBox.end = event.hour + 1;
                if (event.status == "free") {
                    for (var i = nowHour; i >= 0; i--) {
                        if (nowItem.hours[i].isNetwork || nowItem.hours[i].status == "already") {
                            reservedBox.end = nowItem.hours[nowHour].hour + 1;
                            break;
                        }
                        nowItem.hours[i].status = "already";
                    }
                    reservedBox.long = reservedBox.end - reservedBox.start;
                } else {
                    for (var i = (nowHour + 1); i < nowItem.hours.length; i++) {
                        if (nowItem.hours[i].isNetwork) {
                            reservedBox.end = nowItem.hours[i - 1].hour;
                            break;
                        }
                        nowItem.hours[i].status = "free";
                    }
                    console.log(reservedBox.end, reservedBox.start);
                    reservedBox.long = reservedBox.end - reservedBox.start;
                }
            }
            console.log(reservedBox.end);
            // reservedBox.long = reservedBox.end - reservedBox.start + 1;

            setTimeout(function () {
                reservedContentEl.style.display = "block";
            }, 50);
            removeClass(reservedContentEl, "slideOutDown");
            addClass(reservedContentEl, "slideInUp");
        },
        getRoomList: function () {
            showLoading();
            var date = dateBox.year.toString() + "-" + ((dateBox.month.toString().length > 1) ? dateBox.month.toString() : ("0" + dateBox.month.toString())) + "-" + ((dateBox.day.toString().length > 1) ? dateBox.day.toString() : ("0" + dateBox.day.toString()));
            var params = {
                placeid: wxData.placeId,
                people: filterBox.people,
                mainbodyid: wxData.mainBodyId,
                hardware: filterBox.hardware,
                start: filterBox.start,
                end: filterBox.end,
                date: date
            }
            if (this.roomId) {
                params.roomid = this.roomId;
            }
            this.$http({
                method: 'post',
                url: baseRequestUrl + 't=hyfw_schedule_roomFilter',
                params: params,
                credientials: true,
                headers: { 'Content-Type': 'multipart/form-data' },
                emulateJSON: true,
                timeout: 300000,
                progress: function (progress) { }
            }).then(function (response) {
                hideLoading();
                if (response.ok && validate(response.body)) {
                    var result;
                    if (typeof (response.body) == "object") {
                        result = response.body;
                    } else {
                        result = JSON.parse(response.body);
                    }

                    var data;
                    if (result.data) {
                        data = result.data;
                    } else {
                        toast.onToast(result.msg);
                        return;
                    }
                    console.log(data);

                    for (var i = 0; i < data.length; i++) {
                        roomList.items.push({
                            id: data[i].id,
                            thumb: data[i].roomPic,
                            address: data[i].address,
                            zoom: false,
                            zoomClass: "",
                            active: false,
                            activeClass: "",
                            title: data[i].title,
                            hours: [],
                            devices: {
                                projector: (data[i].hardware.indexOf(1001) >= 0) ? true : false,
                                board: (data[i].hardware.indexOf(1002) >= 0) ? true : false,
                                video: (data[i].hardware.indexOf(1003) >= 0) ? true : false,
                                phone: (data[i].hardware.indexOf(1004) >= 0) ? true : false,
                                computer: (data[i].hardware.indexOf(1005) >= 0) ? true : false
                            },
                            people: data[i].acceptPeople
                        });


                        for (var j = 0; j < hoursT.length; j++) {
                            if (data[i].taked.hasOwnProperty(parseInt(hoursT[j].hour))) {
                                var status = "busy";
                                for (var key in data[i].taked[hoursT[j].hour]) {
                                    if (data[i].taked[hoursT[j].hour][key][0] && (data[i].taked[hoursT[j].hour][key][0].userid == wxData.userId)) {
                                        status = "already";
                                    }
                                }
                                roomList.items[roomList.items.length - 1].hours.$set(j, {
                                    isNetwork: true,
                                    hour: hoursT[j].hour,
                                    text: hoursT[j].text,
                                    status: status
                                });
                            } else {
                                roomList.items[roomList.items.length - 1].hours.$set(j, {
                                    isNetwork: false,
                                    hour: hoursT[j].hour,
                                    text: hoursT[j].text,
                                    status: "free"
                                });
                                // roomList.items[roomList.items.length - 1].hours[j] = {
                                // 	hour: hoursT[j].hour,
                                // 	text: hoursT[j].text,
                                // 	status: "free"
                                // };
                            }
                        };
                    };
                }
            }, function (response) {
                hideLoading();
                console.log(response);
            }).catch(function (response) {
                hideLoading();
                console.log(response);
            });
        },
        onDetail: function (msg, event) {
            location.href = "roomDetail.html?id=" + event.id + "&year=" + dateBox.year + "&month=" + dateBox.month + "&day=" + dateBox.day + "&week=" + dateBox.week + "&rand=" + new Date().getTime();
        }
    }
})

var nowDay = {};
nowDay.now = new Date();
nowDay.year = nowDay.now.getFullYear();
nowDay.month = nowDay.now.getMonth() + 1;
nowDay.day = nowDay.now.getDate();
nowDay.week = nowDay.now.getDay();

var dateBox = new Vue({
    el: "#date-box",
    data: {
        dateBoxEl: document.getElementById('date-box'),
        daysListEl: document.getElementById('days-list'),
        disX: 0,
        year: nowDay.year,
        month: nowDay.month,
        day: nowDay.day,
        week: nowDay.week,
        weeks: [{
            id: 0,
            week: "日",
            nowday: "",
            select: ""
        },
        {
            id: 1,
            week: "一",
            nowday: "",
            select: ""
        },
        {
            id: 2,
            week: "二",
            nowday: "",
            select: ""
        },
        {
            id: 3,
            week: "三",
            nowday: "",
            select: ""
        },
        {
            id: 4,
            week: "四",
            nowday: "",
            select: ""
        },
        {
            id: 5,
            week: "五",
            nowday: "",
            select: ""
        },
        {
            id: 6,
            week: "六",
            nowday: "",
            select: ""
        }
        ],
        days: [
            // {
            // 	day: 24,
            // 	week: 7,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 25,
            // 	week: 1,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 26,
            // 	week: 2,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 27,
            // 	week: 3,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 28,
            // 	week: 4,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 30,
            // 	week: 5,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 31,
            // 	week: 6,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 1,
            // 	week: 7,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 2,
            // 	week: 1,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 3,
            // 	week: 2,
            // 	nowday: "nowday",
            // 	select: "select"
            // },
            // {
            // 	day: 4,
            // 	week: 3,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 5,
            // 	week: 4,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 6,
            // 	week: 5,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 7,
            // 	week: 6,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 8,
            // 	week: 7,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 9,
            // 	week: 1,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 10,
            // 	week: 2,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 11,
            // 	week: 3,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 12,
            // 	week: 4,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 13,
            // 	week: 5,
            // 	nowday: "",
            // 	select: ""
            // },
            // {
            // 	day: 14,
            // 	week: 6,
            // 	nowday: "",
            // 	select: ""
            // }
        ]
    },
    ready: function () {
        this.getNowDay();
    },
    methods: {
        getNowDay: function () {
            var _this = this;
            getNowDay(_this, function (data) {
                nowDay.now = new Date(parseInt(data) * 1000);
                nowDay.year = nowDay.now.getFullYear();
                nowDay.month = nowDay.now.getMonth() + 1;
                nowDay.day = nowDay.now.getDate();
                nowDay.week = nowDay.now.getDay();

                selectDay.year = nowDay.now.getFullYear();
                selectDay.month = nowDay.now.getMonth() + 1;
                selectDay.day = nowDay.now.getDate();
                selectDay.week = nowDay.now.getDay();

                _this.year = nowDay.year;
                _this.month = nowDay.month;
                _this.day = nowDay.day;
                _this.week = nowDay.week;

                _this.buildDate();
            });
        },
        buildDate: function () {
            var _this = this;
            buildDate(_this, function () {
                selectDate.innerHTML = _this.month + "月" + _this.day + "日";
                roomList.getRoomList();
            });
        },
        addPrevDate: function () {
            var _this = this;
            addPrevDate(_this);
        },
        addNextDate: function () {
            var _this = this;
            addNextDate(_this);
        },
        onSwitch: function (msg, event) {
            var _this = this;
            switchDate(_this, event, function () {
                selectDate.innerHTML = event.month + "月" + event.day + "日";
                roomList.items = [];
                roomList.getRoomList();
            });
        },
        onSwipe: function () {
            var _this = this;
            onSwipeDate(_this);
        },
        swipeReset: function () {
            var _this = this;
            swipeReset(_this);
        },
        swipeEvent: function () {
            var _this = this;
            swipeEvent(_this, function () {
                roomList.items = [];
                roomList.getRoomList();
            });
        }
    }
})

var reservedBox = new Vue({
    el: "#reserved-box",
    data: {
        id: null,
        isShow: false,
        fullDay: false,
        start: 0,
        end: 0,
        long: 0,
        maxEnd: 19,
        thumb: null,
        devices: [],
        title: null,
        address: null
    },
    methods: {
        offReserved: function (msg, event) {
            onScroll();
            this.id = null;
            this.thumb = null;
            this.title = null;
            this.address = null;
            this.devices = [];
            this.long = 0;
            this.end = 0;
            this.start = 0;
            removeClass(reservedContentEl, "slideInUp");
            addClass(reservedContentEl, "slideOutDown");

            for (var i = 0; i < roomList.items.length; i++) {
                for (var j = 0; j < roomList.items[i].hours.length; j++) {
                    if (!roomList.items[i].hours[j].isNetwork) {
                        roomList.items[i].hours[j].status = "free";
                    }
                }
            }

            setTimeout(function () {
                reservedContentEl.style.display = "none";
                reservedBox.isShow = false;

                for (var i = 0; i < roomList.items.length; i++) {
                    roomList.items[i].active = false;
                    roomList.items[i].activeClass = "";
                }
            }, 300);
        },
        onReserved: function (msg, event) {
            localStorage.removeItem("reservedData");

            var reservedData = {
                roomId: this.id,
                title: this.title,
                address: this.address,
                thumb: this.thumb,
                start: this.start + ":00",
                end: this.end + ":00",
                year: dateBox.year,
                month: dateBox.month,
                day: dateBox.day,
                devices: this.devices
            };

            localStorage.setItem("reservedData", JSON.stringify(reservedData));
            location.href = "meetingNew.html?rank=" + new Date().getTime();
        }
    }
});

function onNowDay() {
    for (var i = 0; i < roomList.items.length; i++) {
        roomList.items[i].active = false;
        roomList.items[i].activeClass = "";
    }

    for (var i = 0; i < dateBox.days.length; i++) {
        dateBox.days[i].select = "";
        if (dateBox.days[i].nowday == "nowday") {
            selectDate.innerHTML = dateBox.month + "月" + dateBox.days[i].day + "日";
            dateBox.days[i].select = "select";
        }
    }
    dateBox.days = [];
    roomList.items = [];
    dateBox.year = nowDay.year;
    dateBox.month = nowDay.month;
    dateBox.day = nowDay.day;
    dateBox.week = nowDay.week;
    dateBox.getNowDay();
}
