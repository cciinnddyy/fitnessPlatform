<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="HMS.Modules.RegistDetail.View" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>


<dnn:DnnCssInclude ID="BootstrapCSS" runat="server" FilePath="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<dnn:DnnJsInclude ID="jQuery" runat="server" FilePath="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js" />
<dnn:DnnJsInclude ID="BootstrapJS" runat="server" FilePath="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" />
<dnn:DnnJsInclude ID="registJS" runat="server" FilePath="~/DesktopModules/RegistDetail/regist.js" />
<dnn:DnnJsInclude ID="momentJS" runat="server" FilePath="~/DesktopModules/dashboard/Scripts/moment.js" />
<dnn:DnnJsInclude ID="datepickjs" runat="server" FilePath="~/DesktopModules/dashboard/Scripts/bootstrap-datetimepicker.min.js"/>
<dnn:DnnCssInclude ID="datepickcss" runat="server" FilePath="~/DesktopModules/dashboard/Content/bootstrap-datetimepicker.css" />



   <div class="container" style="margin-top:50px;">
        <form class="form-horizontal" method="post" action="">
          <fieldset>
       
          <div id="edit_farmer" style="display:none"></div>
          <div class="row">

            <div class="col-md-3 panel panel-heading">Member Details</div>
            <div class="col-md-3 panel panel-heading" id="userid"></div>
            <div class="col-md-4 panel panel-heading" style="display:none; color:red" id="contact_error"></div>
          </div>

   <div class="row form-group">
    <div id="macad" style="display:none">
    <label class="col-md-2 control-label" for="mobile">Device MAC Address</label>
    <div class="col-md-3">
        <div class="input-group">
			<span class="input-group-addon">
			<i class="glyphicon glyphicon-phone"></i>
			</span>
              <input id="mobile" name="mobile" placeholder="xxxxxxxxxxxx" class="form-control input-md ac_mobile" >		
         </div>
	</div>
    </div>
    <label class="col-md-1 control-label" for="role_type">Role</label>
        
		 <div class="col-md-4">
              <label class="radio-inline"><input type="radio" name="role_type" value="trainer">Trainer</label>
              <label class="radio-inline"><input type="radio" name="role_type" value="trainee">Trainee</label>
         </div>
        
       </div>

      <div id="fortrainer" style="display:none" class="row form-group">
         <label class="col-md-1 control-label" for="trainers">Trainer</label>
            <div class="col-md-3">
		     <div class="input-group">
			     <span class="input-group-addon">
			    <i class="glyphicon glyphicon-list"></i>
			  </span>
              <select id="trainers" name="trainers" class="form-control input-md">
                                
              </select>
            </div>
            </div>
            <label class="col-md-1 control-label" for="height_user">Height</label>  
            <div class="col-md-3">
			<div class="input-group">
			<span class="input-group-addon">
			<i class="glyphicon glyphicon-user"></i>
			</span>
              <input id="height_user" name="height_user" placeholder="cm" class="form-control input-md" type="number">
            </div>
		</div>
          <label class="col-md-1 control-label" for="weight_user">Weight</label>  
            <div class="col-md-2">
			<div class="input-group">
			<span class="input-group-addon">
			<i class="glyphicon glyphicon-user"></i>
			</span>
              <input id="weight_user" name="weight_user" placeholder="kg" class="form-control input-md" type="number">
            </div>
		</div>
      </div>
    <div id="fortrainer02" style="display:none" class="form-group row">
        <div class="col-md-4 text-center">
        <label class="col-md-2 control-label">Start Date</label>  
            <div class="form-group">
                <div class='input-group date' id='datetimepicker1'>
                    <input type='text' class="form-control" placeholder="The first day you join the program" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>    
   </div>
</div>
          <div class="form-group row">
              <div class="col-md-8 text-center">
                  <button id="save" type="button" name="save" class="btn btn-large btn-success">Save</button>
              </div>

          </div>          
          </fieldset>
        </form>
       
      </div>

<div class="b">
 <div class="navbar navbar-default navbar-fixed-bottom">
    <div class="container">
     <p class="navbar-text pull-left">© 2016 - Site Built By 
           <a href="http://www.nimbrisk.com">NimbriskTechnology.com</a>
      </p>
      
      <a href="" class="navbar-btn btn-danger btn-xs btn pull-right"> 
      <span class="glyphicon glyphicon-star"></span>  Subscribe on YouTube</a>
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
        
        var WebApi = '<%=connect%>';

        var radiobtns = $('input[type=radio][name=role_type]');
        
        var rolevalues;
        radiobtns.change(function () {


            
            if ($(this).val() == "trainer") {
                $('#macad').hide();
                $('#fortrainer').hide();
                $('#fortrainer02').hide();
                rolevalues = "trainer";
            }
                

            else if ($(this).val() == "trainee") {
                    $('#macad').show();
                    $('#fortrainer').show();
                     $('#fortrainer02').show();
                rolevalues = "trainee";
                    
                }
         

        });


        $('#datetimepicker1').datetimepicker();

        var userID = '<%=userid%>';

        $('#userid').html(String.format("User ID:{0}", userID));

        loadTrainerList(WebApi, '#trainers', '<%=userid%>');

        //userid, tag of role, tag of weight, tag of height, tag of mac, tag of startdate
        getuserinfo(WebApi, '<%=userid%>', 'input[type=radio][name=role_type]', '#weight_user', '#height_user', '#mobile', '.date input');


        var trainerselect = $("#trainers option:selected").val();
        $("#trainers").change(function() {
            $("#trainers option:selected").each(function () {
                trainerselect = $(this).val();
                
            });
   
            })

        


        

        

        

        

        var success = function (data) {
            alert("Changes has been made successfully");
        }

        var savedetail = function () {
            var weight = $('#weight_user').val();
            var height = $('#height_user').val();
            var startdateLocal;
            var rolevalue = $('input[type=radio][name=role_type][checked=checked]').val();
            var macaddress = "C4:3E:BD:71:DE:C2";
            var startdateUTC;
            var startdatelocal;
            var trainerselect = $("#trainers option:selected").val();

            if (startdateLocal != "") {
                
                startdatelocal = $('#datetimepicker1').data('DateTimePicker').viewDate();

                startdateLocal = startdatelocal.format("DD/MM/YYYY HH:mm:ss");
                
                startdateUTC = startdatelocal.utc().format();
                
                
            }

            console.log(rolevalues);
            console.log(rolevalue);
            if (rolevalue == undefined && rolevalues != undefined) {
                rolevalue = rolevalues;
            }

            var data;
            if (rolevalue == "trainer") {
                var registerdetails = new registdetail(rolevalue,'<%=userid%>', null, null, null,null,null,null);
                    
                    data = JSON.stringify(registerdetails);
            };
            var flag = true;
            if (rolevalue == "trainee") {

                if (weight == "" || height == "" || trainerselect == "" || startdateLocal == "" || rolevalue == "") {

                    
                    flag = false;
                }

                var registerdetails = new registdetail(rolevalue, '<%=userid%>', weight, height, trainerselect, startdateLocal, macaddress, startdateUTC);

                

                data = JSON.stringify(registerdetails);

            };

            if (rolevalue == "") {
                alert("select your role!");
            }
            else if (flag == false) {
                alert("Require more information!");
            }
            else {
              
                $.ajax({
                    type: "POST",
                    contentType: 'text/plain; charset=utf-8',
                    url: WebApi + "api/values/saveRegistDetail",
                    data: data,
                    success: success,
                    dataType: "json"
                });
            }
        }

        $('#save').click(savedetail);

        

       


    });
</script>