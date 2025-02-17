
var fieldOfPasswordLogin = $("#password");

$("#eye").on("click", function () {
    var type = fieldOfPasswordLogin.attr("type") === "password" ? "text" : "password";
    fieldOfPasswordLogin.attr("type", type);
    $("#togglePasswordLogin").toggleClass("bi-eye bi-eye-slash");
});
var fieldOfPasswordRegister = $("#password1");

$("#eye1").on("click", function () {
    var type = fieldOfPasswordRegister.attr("type") === "password" ? "text" : "password";
    fieldOfPasswordRegister.attr("type", type);
    $("#togglePasswordRegister").toggleClass("bi-eye bi-eye-slash");
});
var fieldOfPasswordRegister2 = $("#password2");
$("#eye2").on("click", function () {
    var type = fieldOfPasswordRegister2.attr("type") === "password" ? "text" : "password";
    fieldOfPasswordRegister2.attr("type", type);
    $("#togglePasswordRegister2").toggleClass("bi-eye bi-eye-slash");
});



