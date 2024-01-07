$(document).ready(function () {
    $("#fromDate").datepicker({ dateFormat: 'yy-mm-dd' });
    $("#toDate").datepicker({ dateFormat: 'yy-mm-dd' });

    $('#confirmDate').click(function (event) {
        event.preventDefault();
        var from = $('#fromDate').val();
        var to = $('#toDate').val();

        if (from && to) { // Making sure both dates are selected
            $.ajax({
                type: "POST",
                url: "ExplorePage.aspx/ConfirmDates", // Make sure this matches your [WebMethod] in code-behind
                data: JSON.stringify({ startDate: from, endDate: to }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "User information is missing.") {
                        Swal.fire({
                            title: 'User Information Missing',
                            text: 'You need to be signed in to confirm dates. Do you want to sign in now?',
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonText: 'Sign In',
                            cancelButtonText: 'Cancel'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = "SignIn.aspx";
                            }
                        });
                    } else if (response.d === "Success") {
                        // If successful, redirect to the ConfirmationPage.aspx
                        window.location.href = "ConfirmationPage.aspx";                        
                    } else {
                        // If the method returns any message other than "Success", alert the message
                        alert(response.d);
                    }
                },
                error: function (error) {
                    alert("Error: " + error.responseText);
                    // Handle errors
                }
            });
        } else {
            alert("Please select both 'From' and 'To' dates.");
        }
    });
});

$(document).ready(function () {
    $('.district-image').click(function () {
        $('html, body').animate({
            scrollTop: $("#districtDetails").offset().top
        }, 1000);
    });
});