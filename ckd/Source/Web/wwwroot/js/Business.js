$("#businesscreateform").validate({
  rules: {
    Name: {
      required: true,
      maxlength: 50,
      minlength: 3,
    },
  },
  messages: {
    Name: {
      required: "Please enter Business name",
      maxlength: "please enter below 50 characters",
      minlength: "please enter above 3 characters",
    },
  },
  errorElement: "span",
  errorPlacement: function (error, element) {
    error.addClass("invalid-feedback");
    element.closest(".form-group").append(error);
  },
  highlight: function (element, errorClass, validClass) {
    $(element).addClass("is-invalid");
  },
  unhighlight: function (element, errorClass, validClass) {
    $(element).removeClass("is-invalid");
  },
});
function loadGrid() {
  $("#BusinessDatatable").DataTable({
    processing: true,
    serverSide: true,
    filter: true,
    orderCellsTop: true,
    ajax: {
      url: "/Business/BusinessData",
      type: "POST",
      datatype: "json",
      //deferLoading: 57,
      dataSrc: function (json) {
        if (json.data == null) {
          return [];
        }
        return json.data;
      },
    },
    language: {
      emptyTable: "No data available in table", //
      loadingRecords: "Please wait .. ", // default Loading...
      zeroRecords: "No matching records found",
      processing: '<i class="fa fa-spinner fa-spin datatablespinner"></i>',
    },
    columns: [
      {
        data: "createdOn",
        name: "CreatedOn",
        autoWidth: true,
        render: function (data) {
          if (data != null) {
            var date = new Date(data);
            var month = date.getMonth() + 1;
            return (
              date.getDate() +
              "/" +
              (month.toString().length > 1 ? month : "0" + month) +
              "/" +
              date.getFullYear()
            );
          } else {
            return "";
          }
        },
      },
      { data: "name", name: "Business", autoWidth: true },
      {
        mRender: function (data, type, full) {
          return (
            '<a href="@Url.Action("GetById","Business")?Id=' +
            full.id +
            '" ><i  class="fa fa-pencil-square iconbasecolor"></i ></a>' +
            '<a ><i  class="fa fa-trash" onclick="Delete(' +
            full.id +
            ')" style="font-size:20px;color:#CC0033 ;padding-left:30px;"></i></a>'
          );
        },
      },
    ],
    columnDefs: [
      {
        targets: [2], // column index (start from 0)
        orderable: false,
        // set orderable false for selected columns
      },
      {
        targets: [0],
        visible: false,
        searchable: false,
      },
    ],
  });
}
function CustomFilterFunction() {
  dataTable = $("#BusinessDatatable").DataTable();

  dataTable.columns(0).search($("#businessinputfilter").val());
  dataTable.columns(1).search($("#businesscreatedonfilter").val());
  dataTable.draw();
}
function EmptyId() {
  $("#modal_id").val("");
  $("#Name").val("");
  $("#cancelbutton").hide();
  window.location.href = "../Business/Index";
}

function Delete(value) {
  $("#delete_modal_id").val(value);
  $("#flag").val(1);
  $("#BusinessDelete").show();
}
function EmptyFlag() {
  $("#flag").val(0);
  $("#BusinessDelete").hide();
}

function Edit(value) {
  //var data = value.serialize();
  //console.log(data);
  //alert(data);
  $.ajax({
    type: "POST",
    url: "/Business/GetById",
    contentType: "application/x-www-form-urlencoded; charset=UTF-8", // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
    data: { Id: value },
    success: function (result) {
      //alert('Successfully received Data ');
      //console.log(result);
      $("#Name").val(result.name);
      $("#modal_id").val(result.id);
      $("html, body").animate({ scrollTop: 0 }, "fast");
    },
    error: function () {
      alert("Failed to receive the Data");
      console.log("Failed ");
    },
  });
}
