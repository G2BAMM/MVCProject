/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : CustomScripts.js                  '
'  Description      : Manages the local jquery/js funcs ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 16 Dec 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/

var url = "/MVCWebProject/Admin/Gallery/GetJsonGallery/";
var modelURL = "/MVCWebProject/Admin/Vehicle/GetJSONVehicleModels/"
var filePath = "/MVCWebProject/images/uploads/"
var updateModelURL = "/MVCWebProject/Admin/VehicleModel/UpdateModel/"
/*
    //Production/Staging server settings
    var url = "/Admin/Gallery/GetJsonGallery/";
    var modelURL = "/Admin/Vehicle/GetJSONVehicleModels/"
    var filePath = "/images/uploads/"
    var updateModelURL = "/Admin/VehicleModel/UpdateModel/"
*/
var modelId = -1;
var manufacturerId = -1;

$(document).ready(function () {
    //Set the tooltip toggle site wide
    $("body").tooltip({ selector: '[data-toggle=tooltip]' });

    //Accordion control management
    
    $(".toggle-accordion").on("click", function () {
        var accordionId = $(this).attr("accordion-id"),
            numPanelOpen = $(accordionId + ' .collapse.in').length;

        $(this).toggleClass("active");
        $("#main_content_").toggle();
        $("#panel_form_").toggle();
        if (numPanelOpen == 0) {
            openAllPanels(accordionId);
        } else {
            closeAllPanels(accordionId);
        }

        openAllPanels = function (aId) {
            console.log("setAllPanelOpen");
            $(aId + ' .panel-collapse:not(".in")').collapse('show');
        }
        closeAllPanels = function (aId) {
            console.log("setAllPanelclose");
            $(aId + ' .panel-collapse.in').collapse('hide');
        }
    })

    //Fires when we open and close an accordion pane
    $(".panel-title").click(function (e) {
        var idClicked = e.target.id;
        if (!$('#main_content_' + idClicked).is(":visible")) {
            //Reset the model number so that we can handle adding new models
            modelId = -1;
        }
        $('#panel_form_' + idClicked).hide(1000);
        $('#main_content_' + idClicked).show();
        manufacturerId = idClicked;
    });

//Ends document ready function
});


function ToggleModal(control) {
    //Show or hide our modal
    $('#' + control).modal('toggle');
}

function StartProgress(control) {
    if ($('#fileUpload').get(0).files.length > 0) {
        //Show the progress bar when we're uploading a file to the gallery
        $("#" + control).toggle();
        //Submit the form
        $('#uploader').submit();
    }
}

function LoadGallery(control, page, items, pageControl) {
    //This function gets called by the modal gallery when adding/editing a vehicle category and the pagers are clicked on
    $.getJSON(url + page + "/" + items, function (result) {
        //Set the total number of pages available for this amount of images(items)
        var numberOfPages = result["NumberOfPages"];
        //Set the gallery array from JSON
        var gallery = result["Gallery"];
        //Clear any existing images so that we don't append them to the existing ones
        $("#" + control).contents().remove();
        //Iterate through the gallery JSON objects and build the modal and bootstrap objects
        $.each(gallery, function (i, field) {
            $("#" + control).append("<div class='col-sm-3'><img src='"
                + filePath
                + field.SmallThumbnail
                + '' + ""
                + "' class='img-responsive img-thumbnail img-rounded'"
                + ' onclick="SwapImage('
                + field.ImageId
                + ",'"
                + field.ThumbnailImage
                + "');" + '"'
                + " id='"
                + field.ImageId
                + "' /><br /><br /></div>");
            
        });
        
        //Now generate the pagers
        CreatePagers(control, numberOfPages, page, items, pageControl);
    });
    //Unexpected error so advise and escape
    $(document).ajaxError(function (event, request, settings) {
        //Clear any visible thumbnails
        $("#" + control).contents().remove();
        //Display the error message
        $("#" + control).append("<p class='text-danger'>There was an error, please try your request again.</p><p>" + request.statusText + ', ' + request.status + ', ' + settings.url + "</p>");
    });

}

function CreatePagers(control, numberofpages, page, items, pagecontrol) {
    //This function gets called by the modal gallery when adding/editing a vehicle category
    var pagerString = "";
    for (var loopCount = 1; loopCount <= numberofpages; loopCount++) {
        if (loopCount === 1 && page === 1) {
            //Set the 'go to first page' link to disabled as we're on the first page
            pagerString += "<li class='page-item disabled'>";
            pagerString += "<a class='page-link' href='#'>&laquo;</a>";
            pagerString += "</li>";
        }
        else if (loopCount == 1 && page > 1) {
            //Set the 'go to first page' link to enabled as we're not on page 1
            pagerString += "<li class='page-item'>";
            pagerString += '<a class="page-link" href="javascript:LoadGallery(' + "'" + control + "'," + 1 + "," + items + ",'" + pagecontrol + "');" + '"' + ">&laquo;</a>";
            pagerString += "</li>";
        }
        if (loopCount == page) {
            //This is the page we're viewing so disable this number
            pagerString += "<li class='page-item active'>";
            pagerString += "<a class='page-link' href='#'>" + loopCount + "</a>";
            pagerString += "</li>";
        }
        else {
            //Set the link to the page to go to as this is not the page we're currently viewing
            pagerString += "<li class='page-item'>";
            pagerString += '<a class="page-link" href="javascript:LoadGallery(' + "'" + control + "'," + loopCount + "," + items + ",'" + pagecontrol + "');" + '"' + ">" + loopCount + "</a>";
            pagerString += "</li>";
        }
        if (loopCount == numberofpages && page == numberofpages) {
            //Set the 'go to last page' link to disabled as we're on the last page
            pagerString += "<li class='page-item disabled'>";
            pagerString += "<a class='page-link' href='#'>&raquo;</a>";
            pagerString += "</li>";
        }
        else if (loopCount == numberofpages && page < numberofpages) {
            //Set the 'go to last page' to be enabled as we're not on the last page
            pagerString += "<li class='page-item'>";
            pagerString += '<a class="page-link" href="javascript:LoadGallery(' + "'" + control + "'," + numberofpages + "," + items + ",'" + pagecontrol + "');" + '"' + ">&raquo;</a>";
            pagerString += "</li>";
        }
        //Finally we can build our pager list
        $("#" + pagecontrol).contents().remove();
        $("#" + pagecontrol).append(pagerString);
    }
}


function SwapImage(id, thumbnailImage) {
    //This function gets called by the modal gallery when adding/editing a vehicle category
    var src = filePath + thumbnailImage;
    //Change the vehicle ImageId to the one selected (this is the C# model field)
    $("#ImageId").val(id);
    //Set the new image to the one we just selected
    $("#Vehicle").attr("src", src);
    //Close the modal window gracefully when we have chosen our new vehicle image
    ToggleModal('myModal');
}

//This function gets called when a manufacturer is selected on the edit vehicle form
//and is used to rebuild the list of models paired with that manufacturer
function RebuildMenu(control) {
    //Set our dropdown control we're rebuilding as a local var to clear and rebuild list into later
    var localControl = $("#" + control);

    //Always empty the current menu even if the user selected the default item on the manufacturer list
    localControl.empty();

    //Now add a new default item advising the user to select a new model regardless of whether there will be any items
    $("<option />", {
        val: "",
        text: "--- Please Select ---"
    }).appendTo(localControl);
    //alert("Menu was changed! " + $("#ManufacturerID option:selected").val());

    //Check to see if we can attempt to rebuild a new menu list
    if ($("#ManufacturerID").prop("selectedIndex") > 0) {
        //We have not selected the default item on the manufacturer dropdown 
        //so we can attempt to repopulate the list with the related vehicle models from our json request
        $.getJSON(modelURL + $("#ManufacturerID option:selected").val(), function (result) {
            $(result).each(function () {
                $("<option />", {
                    val: this.Id,
                    text: this.Display
                }).appendTo(localControl);
            });
        });
    }
}


/****************** Accordion functions ***************************************/

//This event also fires when the 'Cancel' button is clicked, as well as local calls in this script
function SwitchPanelBody(panelNumber) {
    //Flips the form and list panels around
    if (!$('#main_content_' + panelNumber).is(":visible")){
        //Reset the model number so that we can handle adding new models
        modelId = -1;
    }
    $('#main_content_' + panelNumber).toggle(1000);
    $('#panel_form_' + panelNumber).toggle(1000);
}

//This event fires when the 'Add New Model' button is clicked
function ClearModelName(manufacturerId) {
    var control = $("#Display_" + manufacturerId);

    //Set the model name form field text value
    $(control).val('');

    //Hide any errors present from previous processes
    $("#error_" + manufacturerId).html('');

    //Re-enable the save button in case it was disabled by another process
    $("#save_" + manufacturerId).removeAttr('disabled');

    //Show the form panel now
    SwitchPanelBody(manufacturerId);
}

//Performs the save operation, which will either cause an update or an insert @ SQL,
//dependent on whether a modelId with a value greater than 0 gets passed back
function SaveModel() {
    //Performs the JSON update and rebuild of the lists

    //Make sure we don't just get a bunch of spaces entered by trimming both ends of the input
    var modelName = $("#Display_" + manufacturerId).val().trim();

    if (modelName.length == 0) {
        //Input was an empty string
        $("#error_" + manufacturerId).html('Field cannot be blank!');
        return;
    }

    if (modelName.length > 50) {
        //OvInput was over sized
        $("#error_" + manufacturerId).html('Cannot have more than 50 chars!');
        return;
    }

    //Prevent multiple clicks on the save button while we perform our update
    $("#save_" + manufacturerId).attr('disabled', 'disabled');

    //Set the form field value to the same one as the link text that opened the form
    $("#Display_" + manufacturerId).val(modelName);

    //Make sure we clear any errors from any previous processes
    $("#error_" + manufacturerId).html('');

    //Set a variable to collect our JSON response into
    var getData;

    //Set our ordered list control we're rebuilding as a local var to clear and rebuild list into later
    var localControl = $("#ModelList_" + manufacturerId);

    //Finally try to perform the update
    $.ajax({
        type: "POST",
        url: updateModelURL,
        data: JSON.stringify({ ManufacturerID: manufacturerId, ModelID: modelId, ModelName: modelName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //JSONIfy our response
            getData = JSON.stringify(response);
            
            //If we have a duplicated model name then we will be returned a single text line as JSON so we can straight forwardly parse this and test
            if ($.parseJSON(getData) == "-1") {
                //Generate the error message
                $("#ErrorMessage_" + manufacturerId).html('Duplicate model name for this manufacturer, please edit your model name and try again.');

                //Now display it
                $("#ErrorMessage_" + manufacturerId).toggle();

                //Delay hiding it for a few seconds
                $("#ErrorMessage_" + manufacturerId).delay(4000).fadeOut('slow');

                //Make sure our save button gets re-enabled so that the user can try again
                $("#save_" + manufacturerId).removeAttr('disabled');

            }
            else {
                //Always empty the current list prior to rebuilding
                localControl.empty();

                //Now we can iterate over the items and rebind them to our ordered list
                $.each($.parseJSON(getData), function (i, field) {
                    $(localControl).append('<li><a href="JavaScript: OpenForm(' + manufacturerId + "," + field.Id + "," + "'" + field.Display + "'" + ');"  id="' + field.Id + '"' + ">" + field.Display + '</a></li>');
                    //alert('<li><a href="JavaScript: OpenForm(' + manufacturerId + "," + field.Id + "," + "'" + field.Display + "'" + ');"  id="' + field.Id + '"' + ">" + field.Display + '</a></li>');
                });

                //Show the success message
                $("#Message_" + manufacturerId).toggle();

                //Delay hiding it for a few seconds
                $("#Message_" + manufacturerId).delay(2000).fadeOut('slow');

                //Gracefully switch back to the list of models now
                setTimeout(function () { SwitchPanelBody(manufacturerId); }, 3000);
            }
        },
        
        error: function (response) {
            alert("Error " + response.statusText + ', ' + response.status + ', ' + response.url + response.responseJSON);

            //Generate the error message
            $("#ErrorMessage_" + manufacturerId).html('There was an error updating! Failure reason ' + response.statusText + ' error code ' + response.status);

            //Now display it
            $("#ErrorMessage_" + manufacturerId).toggle();

            //Delay hiding it for a few seconds
            $("#ErrorMessage_" + manufacturerId).delay(4000).fadeOut('slow');

            //Make sure our save button gets re-enabled in case so that the user can try again
            $("#save_" + manufacturerId).removeAttr('disabled');
        }
    });
    
}
    //This fires when a model name is clicked inside an accordion pane
    function OpenForm(ManufacturerID, ModelID, ModelName) {
        //Presents the edit form for the 'vehicle model' that's being edited or added
        manufacturerId = ManufacturerID;
        modelId = ModelID;
        var modelName = ModelName
        var control = $("#Display_" + manufacturerId);

        //Set the 'vehicle model name' form field text value
        $(control).val(modelName);

        //Make sure the saved successfully message is always hidden on a newly opened form
        $("#Message_" + manufacturerId).hide();

        //Make sure our save button gets re-enabled in case it was disabled by another process
        $("#save_" + manufacturerId).removeAttr('disabled');

        //Show the form panel now
        SwitchPanelBody(manufacturerId);
        
    }

/****************** Ends Accordion functions ***************************************/
