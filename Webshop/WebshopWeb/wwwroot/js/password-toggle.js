
var fieldOfPasswordLogin = $("#password");

$("#togglePasswordLogin").on("click", function () {
    var type = fieldOfPasswordLogin.attr("type") === "password" ? "text" : "password";
    fieldOfPasswordLogin.attr("type", type);
    $(this).toggleClass("bi-eye bi-eye-slash");
});
var fieldOfPasswordRegister = $("#password1");

$("#togglePasswordRegister").on("click", function () {
    var type = fieldOfPasswordRegister.attr("type") === "password" ? "text" : "password";
    fieldOfPasswordRegister.attr("type", type);
    $(this).toggleClass("bi-eye bi-eye-slash");
});
var fieldOfPasswordRegister2 = $("#password2");
$("#togglePasswordRegister2").on("click", function () {
    var type = fieldOfPasswordRegister2.attr("type") === "password" ? "text" : "password";
    fieldOfPasswordRegister2.attr("type", type);
    $(this).toggleClass("bi-eye bi-eye-slash");
});



