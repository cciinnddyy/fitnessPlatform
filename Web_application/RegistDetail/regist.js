var registdetail = function (role,userid,weight,height,trainer,startdateLocal,macaddress,startdateUTC) {
    this.role = role;
    this.userid = userid;
    this.weight = weight;
    this.height = height;
    this.trainer = trainer;
    this.startdateLocal = startdateLocal;
    this.macaddress = macaddress;
    this.startdateUTC = startdateUTC;
};


var loadTrainerList = function (WebApi, selectorID,userid) {
    
    $.get(WebApi + "api/values/getTrainers?userid=" + userid, function (data, status) {
        
        
        var datacount = data.length;

        
        
        if (data[datacount - 1].toString() !== "-1") {
            for (var i = 0; i < datacount - 1; i++) {
                if (i.toString() === data[datacount - 1]) {
                    $(selectorID).append($("<option></option>").attr("value", data[i]).attr("selected", "selected").text(data[i]));
                    
                }
                else {
                    $(selectorID).append($("<option></option>").attr("value", data[i]).text(data[i]));
                }
            }
        }
        else {
            for (var j = 0; j < datacount - 1; j++) {
                
                console.log(typeof data[j]);
                $(selectorID).append($("<option></option>").attr("value", data[j]).text(data[j]));
            }
        }
    
    });
};

//var timezonetransfer = function (startdate) {

//    startdate = new Date(Date.parse(startdate));
//    var y = startdate.getYear();
//    var M = startdate.getMonth();
//    var h = startdate.getHours();
//    var m = startdate.getMinutes();
//    var s = startdate.getSeconds();

//    var date = new Date(Date.UTC(y, M, h, m, s, 0));


//    return date.toString();
//};

//webapi, userid, tag of role, tag of weight, tag of height, tag of mac, tag of startdate
var getuserinfo = function (WebApi, userid, role, weight, height, mac, startdate) {

    $.get(WebApi + "api/values/getuserinfo?userid=" + userid, function (data, status) {

        

        if (data !== "") {
            var datas = JSON.parse(data);
            if (datas["role"] === "trainer") {
                $(role + '[value=trainer]').attr('checked', 'checked');
            }
            else if (datas["role"] === "trainee") {
                
                $(role + '[value=trainee]').attr('checked','checked');
           
                $('#macad').show();
                $('#fortrainer').show();
                $('#fortrainer02').show();
                
            }
            $(weight).val(datas["weight"]);
            $(height).val(datas["height"]);
            $(mac).val(datas["macaddress"]);
            $(startdate).val(datas["startdate"]);

            
            
        }
        


    });

};

