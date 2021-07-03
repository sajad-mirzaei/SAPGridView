//-Start-of-SapGridViewJSBind--------------------------------------------------------------------------
var SGVArray = SGVArray !== undefined ? SGVArray : {};
var SGVGlobalVariables = {};
var SGVTableCounter = 1;
var CustomData = {};
var SGVFunctionList = {
    CumulativeSum: { FuncListBuild: ["createdCell", "orderChange"] },
    Calc: { FuncListBuild: ["createdCell"] },
    MiladiToJalali: { FuncListBuild: ["createdCell"] },
    Separator: { FuncListBuild: ["createdCell"] },
    SAPCheckBox: { FuncListBuild: ["createdCell"] },
    OnClick: { FuncListBuild: ["createdCell"] },
    TextFeature: { FuncListBuild: ["createdCell"] }
};
function SapGridViewJSBind(RData, Level, GridTitle) {
    var newRData = JSON.parse(JSON.stringify(RData));
    CustomData = newRData.CustomData;
    /*console.log(newRData);
    console.log("----------------------");*/
    $.each(newRData["Grids"], function (GridName, DataArray) {
        if (Array.isArray(DataArray["data"]) !== true) {
            DataArray["data"] = JSON.parse(DataArray["data"]);
        }
        DataArray.columns.unshift({ title: "#", defaultContent: "", data: "SGVRadifCounter", orderable: false, visible: DataArray.counterColumn });
        var ContainerId = DataArray.containerId;
        var TextGridParameters = base64Encode(JSON.stringify(DataArray.gridParameters));
        //var ExtraPostfix = "-" + SGVTableCounter;
        var ExtraPostfix = "";
        var JoinContainerAndGridName = ContainerId + "_" + GridName;
        var ThisTabID = JoinContainerAndGridName + "_ThisTab" + "_Level" + Level + ExtraPostfix;
        var ThisTabTitle = Level + "- " + GridTitle;
        var ThisTabContentID = JoinContainerAndGridName + "_ThisTabContent" + "_Level" + Level + ExtraPostfix;
        var TabsContainerID = "SGV_" + ContainerId + "Tabs";
        var AllTitleTh = "";
        var ThisColumnDefs = [];
        var ThisTableID = JoinContainerAndGridName + "_Level" + Level + ExtraPostfix;
        var TbodyID = JoinContainerAndGridName + "_Level" + Level + "GridTbody" + "_Level" + Level + ExtraPostfix;
        var TheadID = ThisTableID + "Thead";
        /*console.log({
            ContainerId: ContainerId,
            GridName: GridName,
            ThisTabID: ThisTabID,
            ThisTabTitle: ThisTabTitle,
            ThisTabContentID: ThisTabContentID,
            TabsContainerID: TabsContainerID,
            ThisTableID: ThisTableID,
            TbodyID: TbodyID
        });*/
        if (SGVGlobalVariables[ContainerId] == undefined)
            SGVGlobalVariables[ContainerId] = {};
        SGVGlobalVariables[ContainerId][ThisTableID] = { hasThead: 0, hasTfoot: 0, columns: {}, columnsName: {} };

        if (SGVArray[ContainerId] !== undefined && SGVArray[ContainerId][ThisTableID] !== undefined && parseInt(Level) == 1) {
            $("#" + ContainerId).html("");
            $.each(SGVArray[ContainerId], function (ThisTableID, v) {
                v.TableAPI.clear();
                v.TableAPI.destroy();
                v.TableObject.destroy();
            });
            SGVArray[ContainerId] = {};
        }
        var TotalFunctionDetails = {
            createdCell: [],
            orderChange: []
        };
        var CellIndex = 0;
        var AllColumns = [];
        var rowGrouping = null;
        for (var j = 0; j < DataArray.columns.length; j++) {
            var TempColumn = DataArray.columns[j];
            if (TempColumn != null && TempColumn.rowGrouping !== undefined && TempColumn.rowGrouping !== null && TempColumn.rowGrouping.enable === true) {
                rowGrouping = { rowNumber: j, cssClass: TempColumn.rowGrouping.cssClass };
            }
            if (TempColumn != null /*&& TempColumn.visible == true*/) {
                AllTitleTh += "<th data-tbodyid='" + TbodyID + "'>" + TempColumn.title + "</th>";
                var CellName = TempColumn["data"] ? TempColumn["data"].trim() : "";
                if (TempColumn["functions"] && TempColumn["functions"] !== null && TempColumn["functions"].length > 0) {
                    SGVGlobalVariables[ContainerId][ThisTableID].columnsName[CellName] = CellIndex;
                    $.each(TempColumn["functions"], function (k, FuncArray) {
                        if (SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex] == undefined)
                            SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex] = {};
                        if (FuncArray.funcName == "CumulativeSum") {
                            SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["cumulative"] = 0;
                        } else if (["OnClick", "Calc"].includes(FuncArray.funcName) && [0, 2].includes(parseInt(FuncArray.section))) {
                            SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"] = 0;
                            SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["name"] = CellName;
                            SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["tfootOnClick"] = true;
                            SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerText"] = FuncArray.footerText !== undefined ? FuncArray.footerText : null;
                        }
                        if (SGVFunctionList[FuncArray.funcName]["FuncListBuild"]) {
                            $.each(SGVFunctionList[FuncArray.funcName]["FuncListBuild"], function (k, DTMethodName) {
                                if (TotalFunctionDetails[DTMethodName][CellIndex] == undefined) {
                                    TotalFunctionDetails[DTMethodName][CellIndex] = { CellName: CellName, CellFuncArray: [] }
                                }
                                TotalFunctionDetails[DTMethodName][CellIndex]["CellFuncArray"].push(FuncArray);
                            });
                        }
                    });
                }
                CellIndex++;
                AllColumns.push(TempColumn)
            }
        }
        DataArray.columns = AllColumns;
        TotalFunctionDetails["createdCell"] = TotalFunctionDetails["createdCell"].filter(function (el) {
            return el != null;
        });
        TotalFunctionDetails["orderChange"] = TotalFunctionDetails["orderChange"].filter(function (el) {
            return el != null;
        });
        if (TotalFunctionDetails["createdCell"].length > 0) {
            var tmp = DTCreatedCell_SGV(TotalFunctionDetails["createdCell"], ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId);
            ThisColumnDefs = tmp[0];
            SGVGlobalVariables = tmp[1];
        }
        var TheadTr = "<tr class='DT_TrThead'>" + AllTitleTh + "</tr><tr data-flag='hide' class='DT_TrFilters'>" + AllTitleTh + "</tr>";
        var TfootTr = "<tr class='DT_TrTfoot'>" + AllTitleTh + "</tr><tr data-flag='hide' class='DT_TrFilters'>" + AllTitleTh + "</tr>";
        var TableHtml = "";
        var GridContentHtml_Start = "<div class='SGV_GridContent SGV_ActiveTabContent " + ContainerId + "GridContent' id='" + ThisTabContentID + "'>";
        TableHtml += "<div class='SGV_LoadingContainer'><i class='fa fa-circle-o-notch fa-spin fa-3x fa-fw SGV_LoadingIcon'></i></div>";
        TableHtml += "<table id='" + ThisTableID + "' data-gridparameters='" + TextGridParameters + "' class='Grid SGV_Grids'>";
        TableHtml += "<thead class='DT_Thead " + TheadID + "' id='" + TheadID + "'>" + TheadTr + "</thead>";
        TableHtml += "<tbody id='" + TbodyID + "' ></tbody>";
        TableHtml += "<tfoot class='DT_Tfoot' >" + TfootTr + "</tfoot>";
        TableHtml += "</table>";
        var GridContentHtml_End = "</div>";
        var ThisTab = TabsControl_SGV(ThisTabID, ThisTabContentID, TabsContainerID, Level, GridTitle, ThisTabTitle, ContainerId);

        if (Level == "1" && ThisTab != "Exist") {
            var TableHtml = "<div class='SGV_TabsContainer sortableSection " + ContainerId + "TabsContainer' id='" + TabsContainerID + "'>" + ThisTab + "</div>" + GridContentHtml_Start + TableHtml + GridContentHtml_End;
            $("#" + ContainerId).append(TableHtml);
        } else if (ThisTab == "Exist") {
            $("#" + ThisTabContentID).html(TableHtml);
        } else {
            var TableHtml = GridContentHtml_Start + TableHtml + GridContentHtml_End;
            $("#SGV_" + ContainerId + "Tabs").append(ThisTab);
            $("#" + ContainerId).append(TableHtml);
        }
        var ThisTable = $("#" + ContainerId + " table");
        var SGVDefaultOptions = DataTable_DefaultOptions(DataArray.options);

        SGVDefaultOptions["columnDefs"] = ThisColumnDefs;
        SGVDefaultOptions["columns"] = DataArray.columns;
        SGVDefaultOptions["data"] = DataArray.data;

        //DataArray = Options_SGV(DataArray, DataArray.options);
        if (SGVArray[ContainerId] !== undefined && SGVArray[ContainerId][ThisTableID] !== undefined) {
            //اینجا نباید مجدد سطرها را اضافه کرد و جدول را رسم کرد چرا که بعد از این شرط یکبار دیتاتیبل را صدا میزنیم و تمام داده ها را به آن میدهیم
            SGVArray[ContainerId][ThisTableID]["TableAPI"].clear();
            SGVArray[ContainerId][ThisTableID]["TableAPI"].destroy();
            TabSwitch_SGV(ThisTabID, ThisTabContentID, ContainerId);
            $("#" + TabsContainerID).show();
            $("#" + ThisTabID).html(ThisTabTitle);
        }


        /*Ajax Paganation---
         * 
         * SGVDefaultOptions["processing"] = true;
        SGVDefaultOptions["serverSide"] = true,
            SGVDefaultOptions["ajax"] = {
                "type": "POST",
                "contentType": "application/json; charset=utf-8",
                "url": document.location.origin + document.location.pathname + "/TestMethod",
                "data": function (d) {
                    *//*console.log(d);*//*
return "{ CallBackData:'" + JSON.stringify(d) + "' }";
},
"dataType": "text",
"dataSrc": function (data) {
if (Array.isArray(data) !== true) {
data = JSON.parse(data);
if (Array.isArray(data.d) !== true)
data.d = JSON.parse(data.d);
}
return data.d.data;
},
"cache": false
};*/

        //Row Grouping
        if (rowGrouping !== null) {
            var columnsCount = SGVDefaultOptions.columns.length;
            SGVDefaultOptions["order"] = [[rowGrouping.rowNumber, 'asc']];
            SGVDefaultOptions["drawCallback"] = function (settings) {
                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();
                var last = null;

                api.column(rowGrouping.rowNumber, { page: 'current' }).data().each(function (group, i) {
                    if (last !== group) {
                        $(rows).eq(i).before(
                            '<tr class="group ' + rowGrouping.cssClass + '"><td colspan="' + columnsCount + '">' + group + '</td></tr>'
                        );

                        last = group;
                    }
                });
            };
        }
        var TableObject = $("#" + ThisTableID).DataTable(SGVDefaultOptions);
        ThisTable.closest(".dataTables_wrapper").addClass("DT_Container");
        var ThisTableAPI = new $.fn.dataTable.Api(TableObject);
        var HeaderFiltersThead = DataTable_HeaderFiltersThead(ThisTable, ThisTableID, TbodyID);
        SGV_AddFilters(TbodyID, TheadID);
        DataTable_onChangeFilters();
        DataTable_KeepScrolHeight(ThisTable, DataArray.containerHeight);
        SGVArray[ContainerId] = SGVArray[ContainerId] == undefined ? {} : SGVArray[ContainerId];
        SGVArray[ContainerId][ThisTableID] = { TableId: ThisTableID, TableObject: TableObject, TableAPI: ThisTableAPI };
        if (typeof DataTableCallBackData === "function") {
            DataTableCallBackData({
                TableId: ThisTableID,
                TableObject: TableObject,
                TableAPI: ThisTableAPI,
                AllData: newRData,
                SGVGlobalVariables: SGVGlobalVariables,
                ContainerId: ContainerId
            });
        }
        if (SGVGlobalVariables[ContainerId][ThisTableID].hasThead === 1 || SGVGlobalVariables[ContainerId][ThisTableID].hasTfoot === 1)
            TheadTfootCalc_SGV({
                TableId: ThisTableID,
                TableObject: TableObject,
                TableAPI: ThisTableAPI,
                SGVGlobalVariables: SGVGlobalVariables,
                columns: DataArray.columns
            }, ContainerId);
        DTOrderChage_SGV(TotalFunctionDetails["orderChange"], ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId, TableObject, DataArray.counterColumn, DataArray.columns);
        SGVTableCounter++;
        if (rowGrouping !== null)
            TableObject.columns([rowGrouping.rowNumber]).visible(false, false).draw();
    });
}

function SGV_AddFilters(TbodyID, TheadID) {
    $("#" + TheadID).find(".DT_TrFilters th").each(function (columnIndex, obj) {
        $(this).attr("data-test", columnIndex);
        var title = $(this).text();
        var width = parseInt($(this).width()) + 20;
        var inputTag = "<span class='DT_ColumnSearchContainer'><input style='min-width:" + width + "px;' type='text' class='form-control-sm input-sm DT_ColumnSearch' data-columnnum='" + columnIndex + "' data-tbodyid='" + TbodyID + "' placeholder='" + title + "'></span>";
        $(this).html(inputTag);
    });
}

function CounterColumn_SGV(TableObject) {
    var i = 1;
    TableObject.rows({ order: 'applied' }).every(function (rowIdx, tableLoop, rowLoop) {
        TableObject.cell(rowIdx, 0).data(i++);
    });
}

function DTOrderChage_SGV(DTOrderChangeFunctions, ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId, TableObject, CounterColumn, DataArraycolumns) {
    TableObject.on('order.dt search.dt', function (changeEvent) {
        if (CounterColumn == true)
            CounterColumn_SGV(TableObject);
        if (DTOrderChangeFunctions.length > 0) {
            $.each(DTOrderChangeFunctions, function (c, DTOrderChange) {
                var CellIndex = SGVGlobalVariables[ContainerId][ThisTableID]["columnsName"][DTOrderChange.CellName];
                if (DTOrderChange) {
                    $.each(DTOrderChange["CellFuncArray"], function (kf, CellFunc) {
                        if (CellFunc) {
                            var fn = window[CellFunc.funcName + "_AfterSortChange_SGV"];
                            if (typeof fn === "function") {
                                fn.apply(window, [CellIndex, CellFunc, DTOrderChangeFunctions, ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId, TableObject, DataArraycolumns]);
                            }
                        }
                    });
                }
            });
        }
    }).draw();
}

function DTCreatedCell_SGV(DTCreatedCellFunctions, ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId) {
    var UniqueFunctionCallCheckArray = []; /*اگر این آرایه نباشد هر تابع محاسبه دوبار خوانده میشود*/
    $.each(DTCreatedCellFunctions, function (CellIndex0, DTCreatedCell) {
        var CellIndex = SGVGlobalVariables[ContainerId][ThisTableID]["columnsName"][DTCreatedCell.CellName];
        if (DTCreatedCell) {
            ThisColumnDefs.push({
                targets: CellIndex,
                createdCell: function (td, cellData, rowData, RowIndex, c) {
                    var ThisCellNewData = cellData;
                    $.each(DTCreatedCell["CellFuncArray"], function (kf, FuncArray) {
                        var tmp = FuncArray.funcName + "_ServerCall_SGV";
                        var UniqueFunctionCallCheck = ContainerId + ThisTableID + DTCreatedCell.CellName + tmp + RowIndex + CellIndex + "-" + kf;
                        if (UniqueFunctionCallCheckArray.includes(UniqueFunctionCallCheck) == false) {
                            UniqueFunctionCallCheckArray.push(UniqueFunctionCallCheck);
                            var fn = window[tmp];


                            var tbodyCheck = ["TextFeature", "Separator"].includes(FuncArray.funcName);
                            var section = parseInt(FuncArray.section);


                            if (typeof fn === "function") {

                                if ((tbodyCheck === true && section == 1) || tbodyCheck === false) {

                                    var tmp = fn.apply(window, [td, ThisCellNewData, rowData, FuncArray, SGVGlobalVariables, DTCreatedCell.CellName, ThisTableID, ContainerId]);
                                    ThisCellNewData = tmp[0];
                                    SGVGlobalVariables = tmp[1];
                                    ThisTDNewData = tmp[2];

                                }

                            }
                            $(td).html(ThisTDNewData);
                            rowData[DTCreatedCell.CellName] = $("<span>" + ThisCellNewData + "</span>").text();
                        }
                    });
                }
            });
        }
    });
    delete UniqueFunctionCallCheckArray;
    return [ThisColumnDefs, SGVGlobalVariables];
}

function Separator_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    if ([0, null, 'null', '0', '', ' ', undefined, NaN, 'undefined', 'NaN'].includes(ThisCellNewData) === false) {
        var x = JSON.stringify(cellData);
        var ThisCellNewData = strToFloat(ThisCellNewData);
        if ([null, 0, 2].includes(parseInt(FuncArray.section)) == false)
            ThisCellNewData = ThisCellNewData.toLocaleString(undefined, { maximumFractionDigits: FuncArray.decimalPlaces });
    }
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function TextFeature_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    var ThisTDNewData = td;
    if (FuncArray.condition !== null) {
        ThisTDNewData = cellData;
        var condition = FuncArray.condition;
        var isTrueCssClass = FuncArray.isTrueCssClass;
        var isFalseCssClass = FuncArray.isFalseCssClass;
        var isTrueText = FuncArray.isTrueText;
        var isFalseText = FuncArray.isFalseText;
        var strReplace = FuncArray.strReplace;
        isTrueText = customStrReplace(isTrueText, cellName, ThisCellNewData, true);
        isFalseText = customStrReplace(isFalseText, cellName, ThisCellNewData, true);
        $.each(rowData, function (key, val) {
            valNumber = ["", undefined, NaN, null, 'undefined', 'NaN', 'null'].includes(strToFloat(val)) == false && strToFloat(val) ? strToFloat(val) : val;
            isTrueText = customStrReplace(isTrueText, key, valNumber, true);
            isFalseText = customStrReplace(isFalseText, key, valNumber, true);
            condition = customStrReplace(condition, key, valNumber, true);
        });
        if (eval(condition)) {
            $.each(strReplace, function (key, val) {
                isTrueText = customStrReplace(isTrueText, key, val);
            });
            $(td).addClass(isTrueCssClass);
            var ThisTDNewData = isTrueText;
        } else {
            $(td).addClass(isFalseCssClass);
            var ThisTDNewData = isFalseText;
        }
    } else {
        errorHandling("TextFeatureConditionNotFound");
    }
    /*
     * باید دیتای اصلی فیلد را بدون تغییر برگردانیم، دیتای اصلی نباید دست بخورد
     * این متد فقط جنیه نمایشی دارد چراکه ممکن است با این متد برای اعداد منفی
     * پرانتز بگذاریم یا متن را تبدیل به تگ اچ تی ام ال کنیم
     */
    return [ThisCellNewData, SGVGlobalVariables, ThisTDNewData];
}

function MiladiToJalali_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    if (["", " ", undefined, NaN, null, 'undefined', 'NaN', 'null', "-"].includes(ThisCellNewData) === false) {
        var ThisCellNewData = ThisCellNewData.toString();
        var ThisDate = ThisCellNewData ? ThisCellNewData.trim() : "";
        ThisDate = ThisDate.replace(/-/g, "/").split('.')[0];
        ThisDate = ThisDate.replace("T", " ");
        if (isValidDate(ThisDate)) {
            var dateTime = new Date(Date.parse(ThisDate));
            j = gregorian_to_jalali(new Array(
                dateTime.getFullYear(),
                dateTime.getMonth() + 1,
                dateTime.getDate()
            ));
            var hour = FuncArray.zeroPad && FuncArray.zeroPad == true ? ("0" + dateTime.getHours()).slice(-2) : dateTime.getHours();
            var minute = FuncArray.zeroPad && FuncArray.zeroPad == true ? ("0" + dateTime.getMinutes()).slice(-2) : dateTime.getMinutes();
            var second = FuncArray.zeroPad && FuncArray.zeroPad == true ? ("0" + dateTime.getSeconds()).slice(-2) : dateTime.getSeconds();
            switch (FuncArray.outPut) {
                case 0: //TimeOnly
                    ThisCellNewData = hour + ":" + minute;
                    break;
                case 1: //DateOnly
                    ThisCellNewData = j[0] + "/" + j[1] + "/" + j[2];
                    break;
                case 2: //FullDate
                    ThisCellNewData = j[0] + "/" + j[1] + "/" + j[2] + " " + hour + ":" + minute;
                    break;
                case 3: //TimeOnlyWithSecond
                    ThisCellNewData = hour + ":" + minute + ":" + second;
                    break;
                case 4: //FullDateWithSecond
                    ThisCellNewData = j[0] + "/" + j[1] + "/" + j[2] + " " + hour + ":" + minute + ":" + second;
                    break;
                default:
                    ThisCellNewData = j[0] + "/" + j[1] + "/" + j[2] + " " + hour + ":" + minute + ":" + second;
            }
        }
    }
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function Calc_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    var tmpFormula = FuncArray.formula;
    var tmpOperator = FuncArray.operator;
    var CellIndex = SGVGlobalVariables[ContainerId][ThisTableID]["columnsName"][cellName];
    SGVGlobalVariables[ContainerId][ThisTableID].hasThead = parseInt(FuncArray.section) === 0 && SGVGlobalVariables[ContainerId][ThisTableID].hasThead === 0 ? 1 : SGVGlobalVariables[ContainerId][ThisTableID].hasThead;
    SGVGlobalVariables[ContainerId][ThisTableID].hasTfoot = parseInt(FuncArray.section) === 2 && SGVGlobalVariables[ContainerId][ThisTableID].hasTfoot === 0 ? 1 : SGVGlobalVariables[ContainerId][ThisTableID].hasTfoot;
    if (parseInt(FuncArray.section) === 1) {
        $.each(rowData, function (key, val) {
            valNumber = val;
            if (FuncArray.numericCheck == true)
                valNumber = strToFloat(val);
            tmpFormula = customStrReplace(tmpFormula, key, valNumber, true);
        });
        try {
            ThisCellNewData = eval(tmpFormula);
        } catch (e) {
            if (e instanceof SyntaxError) {
                console.log(e.message);
                console.log("-------------------------");
            }
        }
    }
    if ([0, 2].includes(parseInt(FuncArray.section)) == true && tmpOperator == 0 && FuncArray.formula == null) {//verticalSum
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["name"] = cellName;
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"] = strToFloat(SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"]);
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"] += strToFloat(ThisCellNewData);
    } else if ([0, 2].includes(parseInt(FuncArray.section)) == false && tmpOperator == 0 && FuncArray.formula == null) {
        errorHandling("VerticalSumWithoutSelectFooterSection");
    }
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function OnClick_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var rowAllData = {};
    rowAllData["FuncArray"] = FuncArray;
    rowAllData["RowData"] = rowData;
    var ThisRowData = JSON.stringify(rowAllData);
    var ThisRowData = base64Encode(ThisRowData);
    var cssClass = FuncArray.cssClass ? FuncArray.cssClass : "btn btn-link text-danger p-0 m-0";
    var webMethodName = FuncArray.webMethodName ? FuncArray.webMethodName : "SapGridEvent";
    var hrefLink = FuncArray.hrefLink ? FuncArray.hrefLink : "javascript:void(0)";
    var javaScriptMethodName = FuncArray.javaScriptMethodName ? FuncArray.javaScriptMethodName : null;
    var httpRequestType = FuncArray.httpRequestType ? parseInt(FuncArray.httpRequestType) : 0;
    var ThisCellNewData = cellData;
    if (parseInt(FuncArray.section) == 1 && FuncArray.enable == true) {
        switch (httpRequestType) {
            case 0:
                //Ajax
                ThisCellNewData = "<a class='" + cssClass + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='AjaxClick_SGV(this)'>" + cellData + "</a>";
                break;
            case 1:
                //PageLink
                ThisCellNewData = "<a class='" + cssClass + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' href='" + hrefLink + "'>" + cellData + "</a>";
                break;
            case 2:
                //CallJavaScriptMethod
                ThisCellNewData = "<a class='" + cssClass + "' data-javascriptmethod='" + javaScriptMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='CallJavaScriptMethodClick_SGV(this)'>" + cellData + "</a>";
                break;
            default:
                ThisCellNewData = "<a class='" + cssClass + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='AjaxClick_SGV(this)'>" + cellData + "</a>";
            /*ThisCellNewData = cellData;
            //PostBack type
            //ThisCellNewData = '<a id="cphMain_SapGrid" href="javascript:__doPostBack(\'ctl00$cphMain$SapGrid\',\'\')">ssssssss</a>';
            ThisCellNewData = "<a class='" + cssClass + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "'  onclick='PostBackClick_SGV(this)'>" + cellData + "</a>";
            break;*/
        }
    }
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function SAPCheckBox_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var rowAllData = {};
    rowAllData["FuncArray"] = FuncArray;
    rowAllData["RowData"] = rowData;
    var ThisRowData = JSON.stringify(rowAllData);
    var ThisRowData = base64Encode(ThisRowData);
    var cssClass = FuncArray.cssClass ? FuncArray.cssClass : "btn btn-link text-danger p-0 m-0";
    var webMethodName = FuncArray.webMethodName ? FuncArray.webMethodName : "SapGridEvent";
    var ThisCellNewData = cellData;
    if (FuncArray.enable == true) {
        ThisCellNewData = "<input type='checkbox' class='" + cssClass + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='SAPCheckBoxClick_SGV(this)' >" + cellData;
    }
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function CumulativeSum_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    var CellIndex = SGVGlobalVariables[ContainerId][ThisTableID]["columnsName"][cellName];
    if (FuncArray.sourceField && FuncArray.sourceField !== null) {
        var tmpFormula = FuncArray.sourceField;
        $.each(rowData, function (key, val) {
            valNumber = strToFloat(val);
            tmpFormula = customStrReplace(tmpFormula, key, valNumber, true);
        });
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["cumulative"] += eval(tmpFormula);
        ThisCellNewData = SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["cumulative"];
    } else
        errorHandling("CumulativeSumSourceFieldNotFound");
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function CumulativeSum_AfterSortChange_SGV(CellIndex, CellFunc, DTOrderChangeFunctions, ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId, TableObject, DataArraycolumns) {
    var sum = 0;
    TableObject.rows({ order: 'applied' }).column(CellIndex).nodes().each(function (obj, rowNumber) {
        var tmpFormula = CellFunc.sourceField;
        var row = TableObject.rows({ order: 'applied' }).column(CellIndex).nodes()[rowNumber]._DT_CellIndex.row;
        $.each(DataArraycolumns, function (cIndex, cArray) {
            var cName = cArray.data;
            var cValue = TableObject.cell(row, cIndex).data();
            cValue = strToFloat(cValue);
            tmpFormula = customStrReplace(tmpFormula, cName, cValue, true);
        });
        var v = eval(tmpFormula);
        sum += v;
        TableObject.cell(row, CellIndex).data(sum.toLocaleString(undefined, { maximumFractionDigits: 3 }));
    });
}

function PostBackClick_SGV(obj) {
    //__doPostBack('ctl00$cphMain$am', '');
    //__doPostBack('ctl00$cphMain$SapGrid', '');
    __doPostBack('SapGrid', '');
    //javascript: __doPostBack('ctl00$cphMain$SapGridPostBack', '');
}

function CallJavaScriptMethodClick_SGV(obj) {
    var CallBackData = base64Decode(obj.dataset.row);
    var CallBackData = JSON.parse(CallBackData);
    var ThisTableID = obj.dataset.tableid;
    var JavaScriptMethodName = obj.dataset.javascriptmethod;
    var gridParameters = $("#" + ThisTableID).attr("data-gridparameters");
    var gridParameters = base64Decode(gridParameters);
    CallBackData["GridParameters"] = JSON.parse(gridParameters);
    var fn = window[JavaScriptMethodName];
    if (typeof fn === "function") {
        fn.apply(window, [{ TableID: ThisTableID, CallBackData: CallBackData, ClickedObject: obj }]);
    }
}

function AjaxClick_SGV(obj) {
    var GridTitle = obj.text ? obj.text : "untitled";
    $(".SGV_LoadingContainer").show();
    var CallBackData = base64Decode(obj.dataset.row);
    var CallBackData = JSON.parse(CallBackData);
    var ThisTableID = obj.dataset.tableid;
    var ContainerId = obj.dataset.containerid;
    var cellName = obj.dataset.cellname;
    var ThisWebMethodName = obj.dataset.webmethodname;
    var gridParameters = $("#" + ThisTableID).attr("data-gridparameters");
    var gridParameters = base64Decode(gridParameters);
    CallBackData["GridParameters"] = JSON.parse(gridParameters);
    CallBackData["TableDetails"] = { ContainerId: ContainerId, TableID: ThisTableID, CellName: cellName };
    var CallBackData = JSON.stringify(CallBackData);
    $.ajax({
        type: "POST",
        url: document.location.origin + document.location.pathname + "/" + ThisWebMethodName,
        data: "{ CallBackData: '" + CallBackData + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (ThisWebMethodName == "SapGridEvent") {
                var d = JSON.parse(data.d);
                SapGridViewJSBind(d, JSON.parse(CallBackData).FuncArray.Level, GridTitle);
            }
            else {
                var fn = window[ThisWebMethodName];
                if (typeof fn === "function") {
                    fn.apply(window, [data.d, obj, SGVArray[ContainerId][ThisTableID]]);
                }
            }
            $(".SGV_LoadingContainer").hide();
        },
        error: function (error) {
            console.log(error);
            $(".SGV_LoadingContainer").hide();
            alert("خطایی در ارتباط با سرور وجود دارد");
        }
    });
}

function TheadTfootCalc_SGV(TableInfo, ContainerId) {
    var ThisFooter = $("#" + TableInfo["TableId"]).closest(".dataTables_wrapper").find(".dataTables_scrollFoot");
    ThisFooter.find(".DT_TrTfootCalc").find("th").html("");
    $.each(TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns, function (CellIndex, cell) {
        var ThisValue = cell.footerValue;
        var ThisDisplayValue = "";
        var ThisVal_OpenTag = "";
        var ThisVal_CloseTag = "";
        var ItemCss = TableInfo.columns[CellIndex].className;
        var cellName = cell.name;
        $.each(TableInfo.columns[CellIndex].functions, function (k, FuncArray) {
            if ([2].includes(parseInt(FuncArray.section))) {

                if (FuncArray.funcName == "OnClick") {
                    var rowAllData = {};
                    rowAllData["FuncArray"] = FuncArray;
                    rowAllData["RowData"] = {};
                    var ThisRowData = JSON.stringify(rowAllData);
                    var ThisRowData = base64Encode(ThisRowData);
                    var cssClass = FuncArray.cssClass ? FuncArray.cssClass : "btn btn-link text-danger p-0 m-0";
                    var cellText = FuncArray.footerText != null ? FuncArray.footerText : ThisValue;
                    var webMethodName = FuncArray.webMethodName ? FuncArray.webMethodName : "SapGridEvent";
                    ThisValue = cellText;
                    if (FuncArray.enable == true) {
                        ThisVal_OpenTag += "<a class='" + cssClass + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + TableInfo["TableId"] + "' onclick='AjaxClick_SGV(this)'>"
                        ThisValue = cellText;
                        ThisVal_CloseTag += "</a>";
                    }
                }
                else if (FuncArray.funcName == "Calc" && FuncArray.formula != null) {
                    var tmpFormula = FuncArray.formula;
                    $.each(TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns, function (key, val) {
                        if (val["footerValue"] && val["name"]) {
                            var footerValue = val["footerValue"];
                            var columnName = val["name"];
                            footerValue = customStrReplace(footerValue.toString(), ",", "");
                            tmpFormula = customStrReplace(tmpFormula, columnName, footerValue, true);
                        }
                    });
                    try {
                        ThisValue = eval(tmpFormula);
                    } catch (e) {
                        if (e instanceof SyntaxError) {
                            console.log(tmpFormula);
                            console.log(e.message);
                            console.log("-------------------------");
                        }
                    }
                }
                else if (FuncArray.funcName == "Separator" && ThisValue !== undefined) {
                    ThisDisplayValue = strToFloat(ThisValue).toLocaleString(undefined, { maximumFractionDigits: FuncArray.decimalPlaces });
                }
                else if (FuncArray.funcName == "TextFeature" && FuncArray.condition != null) {

                    var condition = FuncArray.condition;
                    var isTrueCssClass = FuncArray.isTrueCssClass;
                    var isFalseCssClass = FuncArray.isFalseCssClass;
                    var isTrueText = FuncArray.isTrueText;
                    var isFalseText = FuncArray.isFalseText;
                    var strReplace = FuncArray.strReplace;
                    $.each(TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns, function (key, val) {
                        var footerValue = [undefined, NaN, null, 'undefined', 'NaN', 'null', 'false', false, ""].includes(val["footerValue"]) == false ? val["footerValue"] : 0;
                        var columnName = [undefined, NaN, null, 'undefined', 'NaN', 'null', 'false', false, ""].includes(val["name"]) == false ? val["name"] : false;
                        condition = customStrReplace(condition, columnName, footerValue, true);
                        footerValue = columnName == cellName && ThisDisplayValue !== "" ? ThisDisplayValue : footerValue;
                        isTrueText = customStrReplace(isTrueText, columnName, footerValue, true);
                        isFalseText = customStrReplace(isFalseText, columnName, footerValue, true);
                    });
                    if (eval(condition)) {
                        $.each(strReplace, function (key, val) {
                            isTrueText = customStrReplace(isTrueText, key, val);
                        });
                        ThisDisplayValue = [null, undefined, NaN, "", "null"].includes(isTrueText) === false ? isTrueText : ThisValue;
                        ItemCss = [null, undefined, NaN, 'undefined', 'NaN', 'null', ""].includes(isTrueCssClass) === false ? isTrueCssClass : ItemCss;
                    } else {
                        ThisDisplayValue = [null, undefined, NaN, "", "null"].includes(isFalseText) === false ? isFalseText : ThisValue;
                        ItemCss = [null, undefined, NaN, 'undefined', 'NaN', 'null', ""].includes(isFalseCssClass) === false ? isFalseCssClass : ItemCss;
                    }
                }
                TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns[CellIndex]["footerValue"] = ThisValue;
            }
        });
        var TempThisVal = ThisDisplayValue !== "" ? ThisVal_OpenTag + ThisDisplayValue + ThisVal_CloseTag : ThisVal_OpenTag + ThisValue + ThisVal_CloseTag;
        if (["undefined", undefined, "NaN", NaN].includes(TempThisVal) === false)
            ThisFooter.find(".DT_TrTfootCalc th").eq(CellIndex).html(TempThisVal);
        if (ItemCss != "") {
            ThisFooter.find(".DT_TrTfootCalc th").eq(CellIndex).addClass(ItemCss);
        }
    });
    ThisFooter.show();
}

/*function Options_SGV(DataArray, options) {
    if (options !== null) {
        options["lengthMenu"] = JSON.parse(options["lengthMenu"].replace(/'/gi, "\""));
        $.each(options, function (k, v) {
            DataArray[k] = v;
        });
    }
    return DataArray;
}*/

function TabsControl_SGV(ThisTabID, ThisTabContentID, TabsContainerID, Level, GridTitle, ThisTabTitle, ContainerId) {
    if ($("#" + ThisTabID).length > 0) {
        ThisTab = "Exist";
    } else {
        var ThisTab = "<div class='SGV_Tab SGV_ActiveTabTitle " + ContainerId + "Tab' id='" + ThisTabID + "' onclick='TabOnClick_SGV(this)' data-containerid='" + ContainerId + "' data-tabid='" + ThisTabID + "' data-contentid='" + ThisTabContentID + "' > ";
        ThisTab += ThisTabTitle;
        ThisTab += " </div> ";
        //ThisTab += "<i class='fa fa-remove SGV_CloseTab' onclick='CloseTab_SGV(this);' data-tabid='" + ThisTabID + "' data-contentid='" + ThisTabContentID + "'></i>";
    }
    if (parseInt(Level) > 1) {
        TabSwitch_SGV(ThisTabID, ThisTabContentID, ContainerId);
        $("#" + TabsContainerID).show();
    }
    return ThisTab;
}

function CloseTab_SGV(obj) {
    var ThisTabID = obj.dataset.tabid;
    var ThisTabContentID = obj.dataset.contentid;
    $("#" + ThisTabID).hide(500);
    $("#" + ThisTabContentID).hide(500);
    //$("#" + ThisTabContentID).hide(500);

    /*$("#" + ThisTabID).closest(".SGV_TabsContainer").find(".SGV_Tab").eq(0).addClass("SGV_ActiveTabTitle");
    $("#" + ThisTabContentID).parent().find(".SGV_GridContent").eq(0).addClass("SGV_ActiveTabContent");*/
}

function TabOnClick_SGV(obj) {
    var ThisTabID = obj.dataset.tabid;
    var ThisTabContentID = obj.dataset.contentid;
    var ContainerId = obj.dataset.containerid;
    TabSwitch_SGV(ThisTabID, ThisTabContentID, ContainerId);
}

function TabSwitch_SGV(ThisTabID, ThisTabContentID, ContainerId) {
    $("." + ContainerId + "Tab").removeClass("SGV_ActiveTabTitle");
    $("." + ContainerId + "GridContent").removeClass("SGV_ActiveTabContent");
    $("#" + ThisTabID).addClass("SGV_ActiveTabTitle").css("display", "");
    $("#" + ThisTabContentID).addClass("SGV_ActiveTabContent").css("display", "");
}

/*function EventFired_SGV(TableObject) {
    TableObject.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        cell.innerHTML = i + 1;
    });
}*/

//-End-of-SapGridViewJSBind--------------------------------------------------------------------------