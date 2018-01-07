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
var filePath = "/MVCWebProject/images/uploads/"
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
    //This function gets called by the modal gallery when adding/editing a vehicle category
    $.getJSON(url + page + "/" + items, function (result) {
        //Set the total number of pages available for this amount of images(items)
        var numberOfPages = result["NumberOfPages"];
        //Set the gallery array from JSON
        var gallery = result["Gallery"];
        //Clear any existing images so that we don't append them to the existing ones
        $("#" + control).contents().remove();
        //Iterate through the gallery JSON objects and build the modal and bootstrap objects
        $.each(gallery, function (i, field) {
            $("#" + control).append('<div class="col-sm-3"><img src='
                + "'/MVCWebProject/images/uploads/"
                + field.SmallThumbnail
                + "' class='img-responsive img-thumbnail img-rounded'"
                + ' onclick="SwapImage('
                + field.ImageId
                + ",'"
                + field.ThumbnailImage
                + "');"
                + ' id='
                + field.ImageId
                + '" /><br /><br /></div>');
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
    for (var loopCount = 1; loopCount <= numberofpages; loopCount++){
        if (loopCount === 1 && page === 1) {
            //Set first page link to disabled as we're on the first page
            pagerString += "<li class='page-item disabled'>";
            pagerString += "<a class='page-link' href='#'>&laquo;</a>";
            pagerString += "</li>";
        }
        else if(loopCount == 1 && page > 1){
            //Set the first page link to enabled as we're not on page 1
            pagerString += "<li class='page-item'>";
            pagerString += '<a class="page-link" href="javascript:LoadGallery(' + "'" + control + "'," + 1 + "," + items + ",'" + pagecontrol + "');" + '"' + ">&laquo;</a>";
            pagerString += "</li>";
        }
        if (loopCount == page) {
            //set the actual page we're on
            pagerString += "<li class='page-item active'>";
            pagerString += "<a class='page-link' href='#'>" + loopCount + "</a>";
            pagerString += "</li>";
        }
        else {
            //set the link to the page to go to
            pagerString += "<li class='page-item'>";
            pagerString += '<a class="page-link" href="javascript:LoadGallery(' + "'" + control + "'," + loopCount + "," + items + ",'" + pagecontrol + "');" + '"' + ">" + loopCount + "</a>";
            pagerString += "</li>";
        }
        if (loopCount == numberofpages && page == numberofpages) {
            //Set the last page link to disabled as we're on the last page
            pagerString += "<li class='page-item disabled'>";
            pagerString += "<a class='page-link' href='#'>&raquo;</a>";
            pagerString += "</li>";
        }
        else if(loopCount == numberofpages && page < numberofpages){
            //Set the last page to be enabled as we're not on the last page
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
    //Change the vehicle ImageId to the one selected (this the model field)
    $("#ImageId").val(id);
    //Set the new image to the one we just selected
    $("#Vehicle").attr("src", src);
    //Close the modal window gracefully when we have chosen our new vehicle image
    ToggleModal('myModal');
}

function RebuildMenu(control) {
    //Set our dropdown control we're rebuilding as a local var to clear and rebuild list into later
    var localControl = $("#" + control);
    //Always empty the current menu even if the user selected the default item on the manufacturer list
    localControl.empty();
    //Now add a new default item advising the user to select a new model regardless of whether there will be any items
    $("<option />", {
        val: "",
        text: "-- Choose New Model --"
    }).appendTo(localControl);
    //alert("Menu was changed! " + $("#ManufacturerID option:selected").val());

    //Check to see if we can attempt to rebuild a new menu list
    if ($("#ManufacturerID").prop("selectedIndex") > 0) {
        //We have not selected the default item on the manufacturer dropdown 
        //so we can attempt to repopulate the list with the related vehicle models from our json request
        $.getJSON(url + page + "/" + items, function (result) {
            $(data).each(function () {
                $("<option />", {
                    val: this.value,
                    text: this.text
                }).appendTo(localControl);
            });
        });
    }
}