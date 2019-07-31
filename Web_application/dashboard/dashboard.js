var getTrainerOrTrainee = function (WebApi, userid) {
    var role = "";
    var success = function (data) {
        if (data !== "") {
            var jsonOBJ = JSON.parse(data);
            role = jsonOBJ.role;
            console.log(role);
        }
    };
    $.ajax({
        type: "GET",
        async: false,
        url: WebApi + "api/values/getuserinfo?userid=" + userid,
        success: success,
        dataType: "json"
    });

    return role;
    
};

var stduentsGoal = function (studentname) {
    this.studentname = studentname;

};

var goals = function (userid, stepsgoal, calgoals) {
    this.userid = userid;
    this.stepsgoal = stepsgoal;
    this.calgoals = calgoals;
};
var messageentity = function (pk, rk, message) {
    this.pk = pk;
    this.rk = rk;
    this.message = message;
}