// 测试版
// var baseRequestUrl = 'http://dev.tyease.com/api.php?';
// 正式版
var baseRequestUrl = 'http://higi.tyease.com/api.php?';

var aT = document.getElementsByTagName("a");
for (var i = 0; i < aT.length; i++) {
    if (validate(aT[i].href)) {
        aT[i].href = aT[i].href + '&rand=' + new Date().getTime();
    }
}

var loadingEl = document.getElementById('loading');
var loading = new Vue({
    el: '#loading',
    data: {
        isShow: false
    },
    methods: {},
    watch: {
        isShow: function ( val ) {
            if (val) {
                loadingEl.addEventListener('touchstart', function (event) {
                    event.preventDefault();
                });
            }
        }
    }
});

var toast = new Vue({
    el: '#toast',
    data: {
        isShow: false,
        title: null
    },
    methods: {
        onToast: function (title, callback) {
            this.title = title;
            this.isShow = true;
            offScroll();
            setTimeout(function () {
                toast.offToast();
                if (callback) {
                    callback();
                }
            }, 2000);
        },
        offToast: function () {
            this.title = null;
            this.isShow = false;
            onScroll();
        }
    },
    watch: {}
});

Vue.transition('stagger', {
    stagger: function (index) {
        // 每个过渡项目增加 50ms 延时
        // 但是最大延时限制为 300ms
        return Math.min(300, index * 50)
    }
})

// 显示菊花
function showLoading() {
    if (loading.isShow) {
        return;
    }
    loading.isShow = true;
}

showLoading();

// 隐藏菊花
function hideLoading() {
    loading.isShow = false;
}

//验证字段是否存在，并且内容是否合法
function validate(fields) {
    if (!fields || fields.toString().replace(/\s+/g, "") == "" || fields == "null" || fields == "undefined" || fields == 0) {
        return false;
    } else {
        return true;
    }
}

function getWxData() {
    var data = {
        userId: null,
        mainBodyId: null,
        placeId: null,
        corpId: null,
        agentId: null,
        isStaff: null,
        info: {
            name: null,
            department: [],
            position: null,
            gender: null,
            avatar: null
        }
    };
    data.userId = getCookie("UserId");
    if (data.userId) {
        data.mainBodyId = getCookie('MainBodyId');
        data.placeId = getCookie('AddressId');
        data.corpId = getCookie('CorpId');
        data.agentId = getCookie('AgentId');
        data.isStaff = getCookie('isStaff');
        data.info = JSON.parse(decodeURIComponent(decodeURIComponent(getCookie("UserInfo"))));
    } else {
        // location.href = 'http://dev.tyease.com/html/index.php?corpid=wx190450eff118d6e7&agentid=4&mainbodyid=29';
        data.userId = 'xiongqichao2';
        data.mainBodyId = 29;
        data.placeId = 223;
        data.corpId = 'wx190450eff118d6e7';
        data.agentId = 4;
        data.isStaff = 1;
        data.info = {
            name: "王欢",
            department: [
                3
            ],
            position: "职位",
            gender: 1,
            avatar: "http://shp.qpic.cn/bizmp/2dWk6m0NhjQnJ37GOBwesG1icLEpsmDkxQx98p4ROtlkCicwZRBRDYmg/"
        }
    }

    return data;
}

function getNowDay(obj, callback) {
    obj.$http({
        method: 'post',
        url: baseRequestUrl + 't=hyfw_schedule_getCurrentTime',
        params: {},
        credientials: true,
        headers: {
            'Content-Type': 'multipart/form-data'
        },
        emulateJSON: false,
        timeout: 300000,
        progress: function (progress) { }
    }).then(function (response) {
        console.log(response);
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
            callback(data);
        }
    }, function (response) {
        hideLoading();
        console.log(response);
    }).catch(function (response) {
        hideLoading();
        console.log(response);
    });
}

function buildDate(obj, callback) {
    for (var i = 0; i < 7; i++) {
        var year;
        var month;
        var day;
        if (i < selectDay.week) {
            var offset = selectDay.day - (selectDay.week - i);
            if (offset < 1) {
                month = obj.month - 1;
                if (month < 1) {
                    year = obj.year - 1;
                    month = 12;
                } else {
                    year = obj.year;
                }
                day = getMonthDays(year, month) + offset;
            } else {
                month = obj.month;
                year = obj.year;
                day = offset;
            }
        } else if (i > selectDay.week) {
            var offset = selectDay.day + (i - selectDay.week);
            if (offset > getMonthDays(obj.year, obj.month)) {
                day = offset - getMonthDays(obj.year, obj.month);
                month = obj.month + 1;
                if (month > 12) {
                    year = obj.year + 1;
                    month = 1;
                } else {
                    year = obj.year;
                }
            } else {
                month = obj.month;
                year = obj.year;
                day = offset;
            }
        } else {
            year = obj.year;
            month = obj.month;
            day = obj.day;
        }

        var isNowday = '';
        if (year == nowDay.year && month == nowDay.month && day == nowDay.day && i == nowDay.week) {
            isNowday = "nowday";
            obj.weeks[i].nowday = "nowday";
        }

        var isSelect = '';
        if (year == selectDay.year && month == selectDay.month && day == selectDay.day) {
            isSelect = "select";
        }
        obj.days.push({
            day: day,
            week: obj.weeks[i].id,
            year: year,
            month: month,
            nowday: isNowday,
            select: isSelect
        });
    }

    addPrevDate(obj);
    addNextDate(obj);
    obj.onSwipe();
    callback();
}

function addPrevDate(obj) {
    var tempT = [];
    for (var i = 0; i < 7; i++) {
        var offset = obj.days[i].day - 7;
        var year;
        var month;
        var day;
        if (offset < 1) {
            month = obj.days[i].month - 1;
            if (month < 1) {
                year = obj.days[i].year - 1;
                month = 12;
            } else {
                year = obj.days[i].year;
            }
            day = getMonthDays(year, month) + offset;
        } else {
            month = obj.days[i].month;
            year = obj.days[i].year;
            day = offset;
        }

        var now = "";
        if (month == nowDay.month && year == nowDay.year && day == nowDay.day) {
            now = "nowday";
        }

        tempT.push({
            day: day,
            week: obj.weeks[i].id,
            year: year,
            month: month,
            nowday: now,
            select: ""
        });
    }
    for (var i = (tempT.length - 1); i >= 0; i--) {
        obj.days.unshift(tempT[i]);
    }
}

function addNextDate(obj) {
    var tempT = [];
    for (var i = 7; i < 14; i++) {
        var offset = obj.days[i].day + 7;
        var year;
        var month;
        var day;
        if (offset > getMonthDays(obj.days[i].year, obj.days[i].month)) {
            day = offset - getMonthDays(obj.days[i].year, obj.days[i].month);
            month = obj.days[i].month + 1;
            if (month > 12) {
                year = obj.days[i].year + 1;
                month = 1;
            } else {
                year = obj.days[i].year;
            }
        } else {
            month = obj.days[i].month;
            year = obj.days[i].year;
            day = offset;
        }

        var now = "";
        if (month == nowDay.month && year == nowDay.year && day == nowDay.day) {
            now = "nowday";
        }

        tempT.push({
            day: day,
            week: obj.weeks[i - 7].id,
            year: year,
            month: month,
            nowday: now,
            select: ""
        });
    }
    for (var i = 0; i < tempT.length; i++) {
        obj.days.push(tempT[i]);
    }
}

function switchDate(obj, event, callback) {
    if (event.select == "select") {
        return;
    }

    for (var i = 0; i < obj.days.length; i++) {
        obj.days[i].select = "";
    }

    for (var i = 0; i < obj.weeks.length; i++) {
        if (obj.weeks[i].id == event.week) {
            obj.weeks[i].select = "select";
        } else {
            obj.weeks[i].select = "";
        }
    }

    event.select = "select";
    obj.year = event.year;
    obj.month = event.month;
    obj.day = event.day;
    obj.week = event.week;
    callback();
}

function onSwipeDate(obj) {
    var touchStartX = 0;
    var touchEndX = 0;
    obj.disX = 0;

    obj.dateBoxEl.addEventListener('touchstart', function (evt) {
        touchStartX = 0;
        touchEndX = 0;
        var touch = evt.changedTouches[0];
        touchStartX = Number(touch.screenX);
    }, false);
    obj.dateBoxEl.addEventListener('touchmove', function (evt) {
        event.preventDefault();
        var touch = evt.changedTouches[0];
        touchEndX = Number(touch.screenX);
        dateBox.disX = touchEndX - touchStartX;
        dateBox.daysListEl.style.left = ((-100 + (dateBox.disX / dateBox.dateBoxEl.offsetWidth * 100)) + "%");
    }, false);
    obj.dateBoxEl.addEventListener('touchend', function (evt) {
        if (touchEndX == 0) {
            return;
        }
        dateBox.disX = touchEndX - touchStartX;
        addClass(dateBox.daysListEl, "transition");
        if (dateBox.disX >= 50) {
            showLoading();
            for (var i = 0; i < dateBox.days.length; i++) {
                dateBox.days[i].select = "";
            }
            for (var i = 0; i < dateBox.weeks.length; i++) {
                dateBox.weeks[i].nowday = "";
            }
            dateBox.daysListEl.style.left = "0%";
            dateBox.daysListEl.addEventListener("webkitTransitionEnd", dateBox.swipeEvent, false);
        } else if (dateBox.disX <= -50) {
            showLoading();
            for (var i = 0; i < dateBox.days.length; i++) {
                dateBox.days[i].select = "";
            }
            for (var i = 0; i < dateBox.weeks.length; i++) {
                dateBox.weeks[i].nowday = "";
            }
            dateBox.daysListEl.style.left = "-200%";
            dateBox.daysListEl.addEventListener("webkitTransitionEnd", dateBox.swipeEvent, false);
        } else {
            dateBox.daysListEl.style.left = "-100%";
            dateBox.dateBoxEl.removeEventListener("webkitTransitionEnd", dateBox.swipeReset, false);
        }
    }, false);
}

function swipeReset(obj) {
    obj.dateBoxEl.removeEventListener("webkitTransitionEnd", dateBox.swipeReset, false);
    removeClass(obj.daysListEl, "transition");
}

function swipeEvent(obj, callback) {
    obj.dateBoxEl.removeEventListener("webkitTransitionEnd", dateBox.swipeEvent, false);
    removeClass(obj.daysListEl, "transition");
    obj.daysListEl.style.left = "-100%";
    if (obj.disX >= 50) {
        obj.days.splice(14, 7);
        obj.addPrevDate();
    } else if (obj.disX <= -50) {
        obj.days.splice(0, 7);
        obj.addNextDate();
    }

    for (var i = 7; i < 14; i++) {
        if (obj.days[i].week == obj.week) {
            obj.days[i].select = "select";
            obj.year = obj.days[i].year;
            obj.month = obj.days[i].month;
            obj.day = obj.days[i].day;
            obj.week = obj.days[i].week;
            if (selectDate) {
                selectDate.innerHTML = obj.days[i].month + "月" + obj.days[i].day + "日";
            }
        }

        if (obj.days[i].month == nowDay.month && obj.days[i].year == nowDay.year && obj.days[i].day == nowDay.day) {
            for (var j = 0; j < obj.weeks.length; j++) {
                if (obj.weeks[j].id == nowDay.week) {
                    obj.weeks[j].nowday = "nowday";
                }
            }
        }
    }
    callback();
}

// 从当前链接截取需要的参数
function getParameter(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]);
    return null;
}

// 替换换行符
function replaceStringLine(str) {
    return str.replace(/[\r\n]/g, "<br />");
}

//获取链接中的文件名
function getFileName(str) {
    return str.replace(/.*(\/|\\)/, "");
}

// 格式化时间戳，full传入返回日期+时间，未传入仅返回日期
function formatDate(str, full, onlyTime) {
    var dateTime = new Date(parseInt(str) * 1000);
    var year = dateTime.getFullYear();
    var month = dateTime.getMonth() + 1;
    var day = dateTime.getDate();

    if (full || onlyTime) {
        var hour = dateTime.getHours().toString();
        var minute = dateTime.getMinutes().toString();
        var second = dateTime.getSeconds().toString();
        if (minute.length < 2) {
            minute = "0" + minute;
        }

        if (second.length < 2) {
            second = "0" + second;
        }
        if (onlyTime) {
            return hour + ":" + minute;
        } else {
            return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
        }
    } else {
        return year + "-" + month + "-" + day;
    }
}

function getWeek(str) {
    var dateTime = new Date(parseInt(str) * 1000);
    return "星期" + "日一二三四五六".charAt(dateTime.getDay());
}

// 获取某月天数
function getMonthDays(year, month) {
    var day = new Date(year, month, 0);
    //获取天数：
    return day.getDate();
}

// 计算时间戳到当前的差值
function getTimeDistance(str) {
    var now = Date.parse(new Date());
    var dis = parseInt(now) / 1000 - parseInt(str);

    if (dis < 300) {
        return "刚刚";
    } else if (dis < 3600) {
        return parseInt(dis / 60) + "分钟前";
    } else if (dis < 86400) {
        return parseInt(dis / 60 / 60) + "小时前";
    } else if (dis < 31536000) {
        return parseInt(dis / 60 / 60 / 24) + "天前";
    } else {
        return formatDate(str, true, false);
    }
}

function needLogin() {
    api.confirm({
        title: '需要登录',
        msg: '是否前往登录？',
        buttons: ['确定', '取消']
    }, function (ret, err) {
        if (ret) {
            console.log(JSON.stringify(ret));
            if (ret.buttonIndex == 1) {
                api.openWin({
                    name: 'login-win',
                    url: 'widget://html/user/login/index.html',
                    bgColor: '#f1f1f1',
                    bounces: false,
                    scrollToTop: false,
                    vScrollBarEnabled: false,
                    hScrollBarEnabled: false,
                    slidBackEnabled: false,
                    scaleEnabled: false,
                    allowEdit: false,
                    customRefreshHeader: '',
                    pageParam: {
                        backWin: api.winName,
                        backFrame: api.frameName
                    }
                });
            }
        } else {
            alert(JSON.stringify(err));
        }
    });
}

Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val)
            return i;
    }
    return -1;
};

Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

function hasClass(obj, cls) {
    return obj.className.match(new RegExp('(\\s|^)' + cls + '(\\s|$)'));
}

function addClass(obj, cls) {
    if (!this.hasClass(obj, cls)) obj.className += " " + cls;
}

function removeClass(obj, cls) {
    if (hasClass(obj, cls)) {
        var reg = new RegExp('(\\s|^)' + cls + '(\\s|$)');
        obj.className = obj.className.replace(reg, ' ');
    }
}

//获取滚动条当前的位置
function getScrollTop() {
    var scrollTop = 0;
    if (document.documentElement && document.documentElement.scrollTop) {
        scrollTop = document.documentElement.scrollTop;
    } else if (document.body) {
        scrollTop = document.body.scrollTop;
    }
    return scrollTop;
}

//获取当前可是范围的高度
function getClientHeight() {
    var clientHeight = 0;
    if (document.body.clientHeight && document.documentElement.clientHeight) {
        clientHeight = Math.min(document.body.clientHeight, document.documentElement.clientHeight);
    } else {
        clientHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight);
    }
    return clientHeight;
}

//获取文档完整的高度
function getScrollHeight() {
    return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
}

// 添加cookies
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
//    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
}

//读取cookies
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg)) {
        return unescape(arr[2]);
    } else {
        return null;
    }
}

//删除cookies
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null) {
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
//        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + ";path=/";
    }
}

// 禁止滚动
function offScroll() {
    document.addEventListener('touchmove', preventDefault, false);
}

// 移除禁止滚动
function onScroll() {
    document.removeEventListener('touchmove', preventDefault, false);
}

// 禁止默认事件传递，常用于阻止页面滚动
function preventDefault(event) {
    event.preventDefault();
}

// 选择图片文件后，本地预览图
function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}

function initTalkingData() {
    var oHead = document.getElementsByTagName('HEAD').item(0);
    var oScript = document.createElement("script");
    oScript.type = "text/javascript";
    var name = location.host.split(".")[0];
    oScript.src = "http://sdk.talkingdata.com/app/h5/v1?appid=48B68147E7E55B6A0D3D61CF28B7A441&vn=" + name + "&vc=0.1";
    oHead.appendChild(oScript);
}
initTalkingData();

var wxData = getWxData();
