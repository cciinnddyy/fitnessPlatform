<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="HMS.Modules.dashboard.View" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>


<dnn:DnnCssInclude ID="BootstrapCSS" runat="server" FilePath="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<dnn:DnnCssInclude ID="font1" runat="server" FilePath="https://fonts.googleapis.com/css?family=Raleway:400,300,600,800,900" />
<dnn:DnnCssInclude ID="progresscd" runat="server" FilePath="~/DesktopModules/dashboard/tox-progress.css" />
<dnn:DnnJsInclude ID="jQuery" runat="server" FilePath="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js" />

<dnn:DnnJsInclude ID="BootstrapJS" runat="server" FilePath="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" />

<dnn:DnnJsInclude ID="progress" runat="server" FilePath="~/DesktopModules/dashboard/tox-progress.js" />
<dnn:DnnCssInclude ID="tablescs" runat="server" FilePath="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.css" />
<dnn:DnnJsInclude ID="tables" runat="server" FilePath="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.js"/>
<dnn:DnnJsInclude ID="dashboard" runat="server" FilePath="~/DesktopModules/dashboard/dashboard.js" />

<div id="div1" style="width:100%">
<div class="container">
<div class="row">  
<div id="stepsdiv" class="col-md-6">
<h2>Weekly Steps Goal</h2>
<div class="tox-progress" data-size="360" data-thickness="30" data-color="#229922" data-background="#DDFF77" data-progress="30" data-speed="500">
    <div class="tox-progress-content" data-vcenter="true">
        <div style="margin:auto;padding:10px;width:100%">
        
            <p id="progresstext" style="font-size:70px;text-align:center">75% <br />
            <p id="textfordivision" style="font-size:25px;text-align:center">75kcal/100kcal</p> </p>
            
    </div>
    </div>
</div>
</div>
<div id="caloriesdiv" class="col-md-6">
<h2>Weekly Calories Goal</h2>
<div class="tox-progress" data-size="360" data-thickness="30" data-color="#229922" data-background="#DDFF77" data-progress="30" data-speed="500">
    <div class="tox-progress-content" data-vcenter="true">
        <div style="margin:auto;padding:10px;width:100%">
        
            <p id="caltext" style="font-size:70px;text-align:center"><br />
            <p id="caldivision" style="font-size:25px;text-align:center"></p> </p>
            
    </div>
    </div>
</div>
</div>
</div>
</div>
</div>

<div id="div2" style="width:100%">
  <table id="table_id" class="display">
    <thead>
        <tr>
            <th>StudentID</th>
            <th>Steps</th>
            <th>StepGoal</th>
            <th>Calories</th>
            <th>CalGoal</th>
            <th></th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        
    </tbody>
</table>
</div>

<div id="myModal" class="modal">

  <!-- Modal content -->
  <div class="modal-content">
    <span class="close">&times;</span>
    <div class="form-group green-border-focus">
        <textarea class="form-control" placeholder="Send message to your trainer"></textarea>

    </div>
      <div>
        <button type="button" style="margin-left:3px" class="btn btn-info">Send</button>
      </div>
  </div>

</div>
<script type="text/javascript">

    String.format = function() {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {       
    var reg = new RegExp("\\{" + i + "\\}", "gm");             
    s = s.replace(reg, arguments[i + 1]);
    }

    return s;
    }

    

    $(document).ready(function () {

        

        var sf = $.ServicesFramework(<%=ModuleId%>);
        var WebApi = ' <%=connect%>';

        var role = getTrainerOrTrainee(WebApi, '<%=userid%>');

        

        if (role == "trainer") {
            $('#div1').hide();
            var myTable = $('#table_id').DataTable();
            $.get(WebApi + "api/values/getstudents?userid=" + '<%=userid%>', function (data, status) {
                if (data != "") {
                    var jsonobj = JSON.parse(data);
                    console.log(jsonobj);
                    var tbody = $('tbody');
                    var emptyelement =$(document.createDocumentFragment());
                    for (var c = 1; c <= jsonobj.length;c++) {
                        var tr = $('<tr></tr>');
                        var emptyf =$(document.createDocumentFragment());
                        for (var a = 0; a <= 6; a++) {
                            var td;                                
                            if (a == 5) {
                                td = $('<td><button type=button class="btn btn-warning">SAVE</button></td>');
                            }
                            else if (a == 6) {
                                td=$('<td><button type=button class="btn btn-warning">Message</button></td>')
                            }
                            else {
                                td = $('<td></td>');
                            }
                            td.appendTo(emptyf);
                            
                        }

                        tr.append(emptyf);
                        tr.appendTo(emptyelement);
                    }
                    tbody.append(emptyelement);
                    tbody.find('tr:first').remove();
                    
                    for (var i = 1; i <= jsonobj.length; i++) {
                        var count = 0;
                        $(String.format("tr:eq({0}) td:eq({1})", i, count)).text(jsonobj[i - 1].userid);
                        count++;
                        $(String.format("tr:eq({0}) td:eq({1})", i, count)).text(jsonobj[i-1].steps);
                        count++
                        $(String.format("tr:eq({0}) td:eq({1})", i, count)).text(jsonobj[i-1].stepsGoal).attr('contenteditable', 'true');
                        count++;
                        $(String.format("tr:eq({0}) td:eq({1})", i, count)).text(jsonobj[i-1].calories);
                        count++;
                        $(String.format("tr:eq({0}) td:eq({1})", i, count)).text(jsonobj[i-1].calorieGoal).attr('contenteditable', 'true');
                       
                        

                        
                    }
                    $('td:eq(5) button').each(function (index) {

                        $(this).on('click', function () {
                            //alert(index);
                            var userid;
                            var stepsgoal;
                            var calgoal;
                            for (var i = 0; i <= 4; i++) {

                                if (i == 0) {
                                    userid = $(String.format("tr:eq({0}) td:eq({1})", index + 1, i)).text();
                                }

                                if (i == 2) {
                                    stepsgoal = $(String.format("tr:eq({0}) td:eq({1})", index + 1, i)).text();
                                }
                                if (i == 4) {
                                    calgoal = $(String.format("tr:eq({0}) td:eq({1})", index + 1, i)).text();
                                }
                            }

                            var gg = new goals(userid, stepsgoal, calgoal);
                            var strjson = JSON.stringify(gg);

                            var success = function (data) {
                                console.log("success");
                            }

                            $.ajax({
                                url: WebApi + "api/values/goalSetting",
                                method: "POST",
                                data: strjson,
                                contenttype: 'text/plain; charset=utf-8',
                                success: success
                            })

                        });
                    })

                    var modal = $('#myModal');
                    var span = $('.close');
                    var sendbtn = $('#myModal button');

                    var clickbtn;
                    


                    var sending = function (clickbtn) {
                        var userid = $(String.format("tr:eq({0}) td:eq(0)", clickbtn + 1)).text();

                        var pk = String.format("NO-{0}", userid);
                        var rk = '<%=userid%>';
                        var message = textarea.val();
                        var messages = new messageentity(pk, rk, message);

                        var jsonObj = JSON.stringify(messages);

                        var success = function (data) {
                            alert("Message Sent");
                            modal.css("display", "none");

                        }

                        $.ajax({
                            url: WebApi + "api/values/sendMessage",
                            method: "POST",
                            data: jsonObj,
                            contenttype: 'text/plain; charset=utf-8',
                            success: success
                        })
                    };

                    sendbtn.on('click', function () {
                        sending(clickbtn);
                    });

                    

                    var textarea = $('#myModal textarea');
                    span.on('click', function () {
                        modal.css('display','none');
                    })

                   
                        $('td:eq(6) button').each(function (index) {
                            
                        $(this).on('click', function () {
                            clickbtn = index;
                            modal.css('display', 'block');

                            
                        })
                    })
                    
                }
                

                
            })
            
 
        }
        else {

            $('#div2').hide();


            $.get(WebApi + "api/values/getWeeklyGoals?userid=" + '<%=userid%>', function (data, status) {
                console.log(data);

                var weekgoalobj = $.parseJSON(data);

                var stepsGoal = weekgoalobj.stepsGoal;
                var calorieGoal = weekgoalobj.calorieGoal;
                var steps = weekgoalobj.steps;
                var calories = weekgoalobj.calories;
                var proportionsteps = steps / stepsGoal * 100;

                if (stepsGoal == 0) {
                    proportionsteps = 100;

                }

                $('#stepsdiv .tox-progress').attr('data-progress', proportionsteps);
                var percentagetext = String.format("{0}%", proportionsteps.toFixed(2));
                $('#progresstext').text(percentagetext);
                var textfordivision = String.format("{0} /{1} steps", steps, stepsGoal);
                $('#textfordivision').text(textfordivision);
                var proportioncal = calories / calorieGoal * 100;

                if (calorieGoal == 0) {
                    proportioncal = 100;
                }

                $('#caloriesdiv .tox-progress').attr('data-progress', proportioncal);
                var percentagetext = String.format("{0}%", proportioncal.toFixed(2));
                $('#caltext').text(percentagetext);
                var textfordivision = String.format("{0} kcal/{1} kcal ", calories, calorieGoal);
                $('#caldivision').text(textfordivision);

                ToxProgress.create();

                ToxProgress.animate();


            })

            




        } 
    })

    
    

</script>