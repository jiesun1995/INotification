$(function () {
    var createModal = new abp.ModalManager(abp.appPath + 'Task/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Task/EditModal');

    var dataTable = $('#TaskTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: false,
            searching: false,
            scrollX: false,
            processing: true,
            ordering: false,
            ajax: abp.libs.datatables.createAjax(iNotificationWeb.services.task.getList),
            columnDefs: [
                {
                    title: "任务名称",
                    data: "name",
                },
                {
                    title: "描述",
                    data: "description"
                },
                {
                    title: "开始时间",
                    data: "beginTime",
                    dataFormat: "datetime"
                },
                {
                    title: "结束时间",
                    data: "endTime",
                    dataFormat: "datetime"
                },
                {
                    title: "上次执行时间",
                    data: "previousFireTime",
                    dataFormat: "datetime"
                },
                {
                    title: "下次执行时间",
                    data: "nextFireTime",
                    dataFormat: "datetime"
                },
                {
                    title: "间隔",
                    data: "interval",
                },
                {
                    title: "类型",
                    data: "jobType",
                },
                {
                    title: "状态",
                    data: "displayState",
                },
            ]
        })
    );
})


