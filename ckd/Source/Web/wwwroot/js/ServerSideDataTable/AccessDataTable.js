function loadGrid() {
  try
  {
  $("#AccessDataTable").DataTable({
    //"processing": true,
    serverSide: true,
    filter: true,
    orderCellsTop: true,
    ajax: {
      url: "/Access/GetAccessDataForDataTable",
      type: "POST",
      datatype: "json",
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
      processing:
        '<i class="fa fa-spinner fa-spin" style="font-size:24px;color:rgb(75, 183, 245);"></i>',
    },
    columns: [
      {
        data: "createdOn",
        name: "Created On",
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
      { data: "name", name: "Access", autoWidth: true },
      
      {
        mRender: function (data, type, full) {
          return (
            '<i  class="fa fa-pencil-square" onclick=Edit("' + full.id + '"); style="font-size:20px;color:#216477;"); ></i >' +          '<i  class="fa fa-trash" style="font-size:20px;color:#CC0033 ;padding-left:30px;" onclick=Delete("' +   full.id +   '");></i>'
          );
        },
      },
    ],
    columnDefs: [
      {
        "targets": [2], // column index (start from 0)
        "orderable": false, // set orderable false for selected columns
      },
      {
        "targets": [0],
        "visible": false,
        "searchable": false
    }
    ],
  });
}
catch(err)
{
  console.log(err);
}
}

function CustomFilterFunction() {
  dataTable = $("#AccessDataTable").DataTable();

  daaTable.columns(1).search($("#businesscreatedonfilter").val());
  dataTable.draw();
}

function Edit(value) {
  Swal.fire({
    title: "<h4 style='color:#0c3f50;';>Are you sure To update</h4> ",
    type: "info",
    showCancelButton: true,
    confirmButtonText: "Update",
    confirmButtonColor: "#ff0055",
    cancelButtonColor: "#999999",
    reverseButtons: true,
    customClass: "swal-wide",

    focusConfirm: false,
    focusCancel: true,
  }).then((result) => {
    /* Read more about isConfirmed, isDenied below */
    if (result.isConfirmed) {
      window.location.href = "/Access/Index?Id=" + value + "";
    } else if (result.isDenied) {
      return false;
    }
  });
}

function Delete(value) {
  Swal.fire({
    title: "<h4 style='color:#0c3f50;';>Are you sure To Delete</h4> ",
    type: "info",
    showCancelButton: true,
    confirmButtonText: "Delete",
    confirmButtonColor: "#ff0055",
    cancelButtonColor: "#999999",
    reverseButtons: true,
    customClass: "swal-wide",

    focusConfirm: false,
    focusCancel: true,
  }).then((result) => {
    /* Read more about isConfirmed, isDenied below */
    if (result.isConfirmed) {
      $.ajax({
        type: "GET",
        url: "/Access/Delete/",
        data: { entityId: value },
        dataType: "json",
        success: function (data) {
          Swal.fire("Deleted", " <h4  style = 'color : #0c3f50; ' >Deleted Successfully  .... </h4>", "success");

          $("#AccessDataTable").DataTable().ajax.reload();
        },
      });
    }
  });
}

function ClearValues() {
  document.getElementById("Id").value = "";
  document.getElementById("CreatedOn").value = "";
  document.getElementById("ModifiedOn").value = "";
  document.getElementById("Name").value = "";
  document.getElementById("Description").value = "";
  document.getElementById("submitButton").value = "Create";

}
if (
  document.getElementById("Id").value != "" &&
  document.getElementById("Id").value != null && document.getElementById("Id").value != 0
)
  document.getElementById("submitButton").value = "Update";

loadGrid();
