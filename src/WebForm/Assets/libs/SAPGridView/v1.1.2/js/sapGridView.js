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

function SapGridViewJSBind(RData, Level, GridFirstText) {
    var newRData = JSON.parse(JSON.stringify(RData));
    CustomData = newRData.CustomData;
    $.each(newRData["Grids"], function (GridName, DataArray) {
        if (Array.isArray(DataArray["data"]) !== true) {
            DataArray["data"] = JSON.parse(DataArray["data"]);
        }
        var counterForColumns = {
            title: "#",
            defaultContent: "",
            data: "SGVRadifCounter",
            orderable: false,
            visible: DataArray.counterColumn,
            dropDownFilter: true,
            rowGrouping: false
        };
        if (DataArray.columns.length == 0 && DataArray["data"] != null && DataArray["data"].length > 0) {
            DataArray.columns.push(counterForColumns);
            $.each(DataArray["data"][0], function (k, v) {
                DataArray.columns.push({
                    title: k,
                    defaultContent: "",
                    data: k,
                    orderable: true
                });
            });
        } else {
            DataArray.columns.unshift(counterForColumns);
        }
        var ContainerId = DataArray.containerId;
        var TextGridParameters = SGV_Base64Encode(JSON.stringify(DataArray.gridParameters));
        //var ExtraPostfix = "-" + SGVTableCounter;
        var ExtraPostfix = "";
        var JoinContainerAndGridName = ContainerId + "_" + GridName;
        var ThisTabID = JoinContainerAndGridName + "_ThisTab" + "_Level" + Level + ExtraPostfix;
        //GridFirstText = [null, undefined, NaN, ""].includes(DataArray.gridTitle) === false ? DataArray.gridTitle : GridFirstText;
        var ThisTabTitle = Level + "- " + GridFirstText;
        var ThisTabContentID = JoinContainerAndGridName + "_ThisTabContent" + "_Level" + Level + ExtraPostfix;
        var TabsContainerID = "SGV_" + ContainerId + "Tabs";
        var AllTitleTh = "";
        var AllFooterTh = "";
        var AllComplexedTh = "";
        var ThisColumnDefs = [];
        var ThisTableID = JoinContainerAndGridName + "_Level" + Level + ExtraPostfix;
        var TbodyID = JoinContainerAndGridName + "_Level" + Level + "GridTbody" + "_Level" + Level + ExtraPostfix;
        var TheadID = ThisTableID + "Thead";
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
        var cellsToBeMerged = [];
        var numberOfUnVisibleCells = 0;
        var mainColumnsTitle = {};
        for (var j = 0; j < DataArray.columns.length; j++) {
            var TempColumn = DataArray.columns[j];
            if (TempColumn != null && TempColumn.rowGrouping !== undefined && TempColumn.rowGrouping !== null && TempColumn.rowGrouping.enable === true) {
                rowGrouping = { rowNumber: j, cssClass: TempColumn.rowGrouping.cssClass };
            }
            if (TempColumn != null /*&& TempColumn.visible == true*/) {
                var CellName = TempColumn["data"] ? TempColumn["data"].trim() : "";
                AllTitleTh += "<th data-tbodyid='" + TbodyID + "'>" + TempColumn.title + "</th>";
                AllFooterTh += "<th data-tbodyid='" + TbodyID + "' id='Footer_" + ThisTableID + "_" + CellName + "'>" + TempColumn.title + "</th>";
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
                AllColumns.push(TempColumn);

                mainColumnsTitle[CellName] = TempColumn.title;
                //-start-headerComplex------------------
                if (TempColumn.visible == false)
                    numberOfUnVisibleCells++;
                if (DataArray.headerComplex != null && DataArray.headerComplex.length > 0) {
                    var temp = SGV_HeaderComplex(DataArray, CellName);
                    if (cellsToBeMerged.includes(CellName)) {
                    }
                    else if (temp.title != "") {
                        cellsToBeMerged = temp.columnsToBeMerged;
                        var colspan = cellsToBeMerged.length + numberOfUnVisibleCells;
                        numberOfUnVisibleCells = 0;
                        AllComplexedTh += "<th rowspan='1' colspan='" + colspan + "'>" + temp.title + "</th> ";
                    }
                    else if (TempColumn.visible !== false) {
                        AllComplexedTh += "<th rowspan='1' colspan='1'>" + TempColumn.title + "</th> ";
                    }
                }
                //-end-headerComplex-------------------------------
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
            var tmp = SGV_DTCreatedCell(TotalFunctionDetails["createdCell"], ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId);
            ThisColumnDefs = tmp[0];
            SGVGlobalVariables = tmp[1];
        }
        var TheadTr = "";
        TheadTr += AllComplexedTh !== "" ? " <tr class='DT_TrThead'> " + AllComplexedTh + " </tr> " : "";
        TheadTr += "<tr class='DT_TrThead'>" + AllTitleTh + "</tr><tr data-flag='hide' class='DT_TrFilters'>" + AllTitleTh + "</tr>";
        var TfootTr = "<tr class='DT_TrTfoot'>" + AllTitleTh + "</tr><tr data-flag='hide' class='DT_TrFilters'>" + AllFooterTh + "</tr>";
        var TableHtml = "";
        var GridContentHtml_Start = "<div class='SGV_GridContent SGV_ActiveTabContent " + ContainerId + "GridContent' id='" + ThisTabContentID + "'>";
        TableHtml += "<div class='SGV_LoadingContainer'><i class='fa fa-circle-o-notch fa-spin fa-3x fa-fw SGV_LoadingIcon'></i></div>";
        TableHtml += "<table id='" + ThisTableID + "' data-gridparameters='" + TextGridParameters + "' class='Grid SGV_Grids'>";
        TableHtml += "<thead class='DT_Thead " + TheadID + "' id='" + TheadID + "'>" + TheadTr + "</thead>";
        TableHtml += "<tbody id='" + TbodyID + "' ></tbody>";
        TableHtml += "<tfoot class='DT_Tfoot' >" + TfootTr + "</tfoot>";
        TableHtml += "</table>";
        var GridContentHtml_End = "</div>";
        var ThisTab = SGV_TabsControl(ThisTabID, ThisTabContentID, TabsContainerID, Level, ThisTabTitle, ContainerId);

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
        var SGVDefaultOptions = SGV_DefaultOptions(DataArray.options, DataArray.gridTitle, ThisTableID, DataArray.customizeButtons);

        SGVDefaultOptions["columnDefs"] = ThisColumnDefs;
        SGVDefaultOptions["columns"] = DataArray.columns;
        SGVDefaultOptions["data"] = DataArray.data;

        //DataArray = Options_SGV(DataArray, DataArray.options);
        if (SGVArray[ContainerId] !== undefined && SGVArray[ContainerId][ThisTableID] !== undefined) {
            //اینجا نباید مجدد سطرها را اضافه کرد و جدول را رسم کرد چرا که بعد از این شرط یکبار دیتاتیبل را صدا میزنیم و تمام داده ها را به آن میدهیم
            SGVArray[ContainerId][ThisTableID]["TableAPI"].clear();
            SGVArray[ContainerId][ThisTableID]["TableAPI"].destroy();
            SGV_TabSwitch(ThisTabID, ThisTabContentID, ContainerId);
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

        //Bind Table
        var TableObject = $("#" + ThisTableID).DataTable(SGVDefaultOptions);
        ThisTable.closest(".dataTables_wrapper").addClass("DT_Container");
        var ThisTableAPI = new $.fn.dataTable.Api(TableObject);

        //Add Extra Settings
        var t = 0
        TableObject.on("draw", function () {

            new charts({
                grid: { charts: DataArray.charts },
                tableObject: TableObject,
                mainColumnsTitle: mainColumnsTitle
            });

            if (t > 0) {
                SGV_AfterFilter_TheadTfootCalc(TableInfo);
            }
            t = 1;
        });
        var TableInfo = {
            TableId: ThisTableID,
            TableObject: TableObject,
            TableAPI: ThisTableAPI,
            SGVGlobalVariables: SGVGlobalVariables,
            Columns: DataArray.columns,
            ContainerId: ContainerId
        };
        var HeaderFiltersThead = SGV_AddGeneralSearch(ThisTable, TableInfo, TbodyID, DataArray.options);
        if (DataArray.options["dropDownFilterButton"] === true || DataArray.options["columnsSearchButton"] === true) {
            SGV_AddFilters(ThisTableID, TbodyID, TheadID, DataArray);
        }
        if (DataArray.options["dropDownFilterButton"] === true)
            SGV_FillDropDownFilters(ThisTableAPI, TheadID, DataArray.columns);

        SGV_OnChangeFilters(TableInfo);
        SGV_HeightControl(ThisTable, DataArray.containerHeight);
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
            SGV_TheadTfootCalc(TableInfo);

        SGV_DTOrderChage(TotalFunctionDetails["orderChange"], ThisColumnDefs, TableInfo, DataArray.counterColumn);
        SGVTableCounter++;
        if (rowGrouping !== null)
            TableObject.columns([rowGrouping.rowNumber]).visible(false, false).draw();

        new charts({
            grid: { charts: DataArray.charts },
            tableObject: TableObject,
            mainColumnsTitle: mainColumnsTitle
        });
    });
}

function Separator_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    if ([0, null, 'null', '0', '', ' ', undefined, NaN, 'undefined', 'NaN'].includes(ThisCellNewData) === false) {
        var ThisCellNewData = SGV_StrToFloat(ThisCellNewData);
        if ([null, 0, 2].includes(parseInt(FuncArray.section)) == false) {
            var minFraction = 0;
            var maxFraction = 0;
            if (!Number.isInteger(ThisCellNewData)) {
                /*مینیمم نباید یک باشد، چراکه فرمت عددی برای اکسل غیرقابل محاصبه می شود، یا هیچ عددی بعد ممیز نباشد یا بیشتر از یک عدد باید باشد*/
                minFraction = FuncArray.minimumFractionDigits > FuncArray.maximumFractionDigits ? FuncArray.maximumFractionDigits : FuncArray.minimumFractionDigits;
                maxFraction = FuncArray.maximumFractionDigits;
                if (maxFraction > 1 && minFraction < 1) {
                    minFraction = 2;
                }
                else if (maxFraction == 1) {
                    ThisCellNewData = SGV_StrToFloat(Number(ThisCellNewData).toFixed(1));
                    minFraction = 2;
                    maxFraction = 2;
                }
            }
            ThisCellNewData = ThisCellNewData.toLocaleString(FuncArray.locales, {
                minimumFractionDigits: minFraction,
                maximumFractionDigits: maxFraction
            });
        }
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
        var numericCheckInText = FuncArray.numericCheckInText;
        var numericCheckInCondition = FuncArray.numericCheckInCondition;
        isTrueText = FuncArray.isTrueText != null ? SGV_CustomStrReplace(isTrueText, cellName, ThisCellNewData, true) : ThisCellNewData;
        isFalseText = FuncArray.isFalseText != null ? SGV_CustomStrReplace(isFalseText, cellName, ThisCellNewData, true) : ThisCellNewData;
        $.each(rowData, function (key, val) {
            var valNumber = ["", undefined, NaN, null, 'undefined', 'NaN', 'null'].includes(SGV_StrToFloat(val)) == false && SGV_StrToFloat(val) ? SGV_StrToFloat(val) : val;
            var vTest = numericCheckInText ? valNumber : val;
            var vCondition = numericCheckInCondition ? valNumber : val;


            if (FuncArray.isTrueText !== null)
                isTrueText = SGV_CustomStrReplace(isTrueText, key, vTest, true);
            if (FuncArray.isFalseText !== null)
                isFalseText = SGV_CustomStrReplace(isFalseText, key, vTest, true);
            condition = SGV_CustomStrReplace(condition, key, vCondition, true);
        });
        if (eval(condition)) {
            $.each(strReplace, function (key, val) {
                isTrueText = SGV_CustomStrReplace(isTrueText, key, val);
            });
            $(td).addClass(isTrueCssClass);
            var ThisTDNewData = isTrueText;
        } else {
            $(td).addClass(isFalseCssClass);
            var ThisTDNewData = isFalseText;
        }
    } else {
        SGV_ErrorMessage("TextFeatureConditionNotFound");
    }
    /*
     * ThisCellNewData ممکن است با تغییر دیتای اصلی
     *مشکلی ایجاد شود مثلا  برای اعداد منفی
     * پرانتز بگذاریم یا متن را تبدیل به تگ اچ تی ام ال کنیم
     */
    if (FuncArray.changeOriginalData == true)
        ThisCellNewData = ThisTDNewData;
    return [ThisCellNewData, SGVGlobalVariables, ThisTDNewData];
}

function MiladiToJalali_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var ThisCellNewData = cellData;
    if (["", " ", undefined, NaN, null, 'undefined', 'NaN', 'null', "-"].includes(ThisCellNewData) === false) {
        var ThisCellNewData = ThisCellNewData.toString();
        var ThisDate = ThisCellNewData ? ThisCellNewData.trim() : "";
        ThisDate = ThisDate.replace(/-/g, "/").split('.')[0];
        ThisDate = ThisDate.replace("T", " ");
        if (SGV_IsValidDate(ThisDate)) {
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
                valNumber = SGV_StrToFloat(val);
            tmpFormula = SGV_CustomStrReplace(tmpFormula, key, valNumber, true);
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
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"] = SGV_StrToFloat(SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"]);
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["footerValue"] += SGV_StrToFloat(ThisCellNewData);
    } else if ([0, 2].includes(parseInt(FuncArray.section)) == false && tmpOperator == 0 && FuncArray.formula == null) {
        SGV_ErrorMessage("VerticalSumWithoutSelectFooterSection");
    }
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function OnClick_ServerCall_SGV(td, cellData, rowData, FuncArray, SGVGlobalVariables, cellName, ThisTableID, ContainerId) {
    var rowAllData = {};
    rowAllData["FuncArray"] = FuncArray;
    rowAllData["RowData"] = rowData;
    var ThisRowData = JSON.stringify(rowAllData);
    var ThisRowData = SGV_Base64Encode(ThisRowData);
    var cssClass = FuncArray.cssClass ? FuncArray.cssClass : "btn btn-link text-danger p-0 m-0";
    var webMethodName = FuncArray.webMethodName ? FuncArray.webMethodName : "SapGridEvent";
    var hrefLink = FuncArray.hrefLink ? FuncArray.hrefLink : "javascript:void(0)";
    var javaScriptMethodName = FuncArray.javaScriptMethodName ? FuncArray.javaScriptMethodName : null;
    var nextTabTitle = FuncArray.nextTabTitle ? FuncArray.nextTabTitle : "";
    var httpRequestType = FuncArray.httpRequestType ? parseInt(FuncArray.httpRequestType) : 0;
    var ThisCellNewData = cellData;
    if (parseInt(FuncArray.section) == 1 && FuncArray.enable == true) {
        switch (httpRequestType) {
            case 0:
                //Ajax
                ThisCellNewData = "<a class='" + cssClass + "' data-nexttabtitle='" + nextTabTitle + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='SGV_AjaxClick(this)'>" + cellData + "</a>";
                break;
            case 1:
                //PageLink
                ThisCellNewData = "<a class='" + cssClass + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' href='" + hrefLink + "'>" + cellData + "</a>";
                break;
            case 2:
                //CallJavaScriptMethod
                ThisCellNewData = "<a class='" + cssClass + "' data-javascriptmethod='" + javaScriptMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='SGV_CallJavaScriptMethodClick(this)'>" + cellData + "</a>";
                break;
            default:
                ThisCellNewData = "<a class='" + cssClass + "' data-nexttabtitle='" + nextTabTitle + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "' onclick='SGV_AjaxClick(this)'>" + cellData + "</a>";
            /*ThisCellNewData = cellData;
            //PostBack type
            //ThisCellNewData = '<a id="cphMain_SapGrid" href="javascript:__doPostBack(\'ctl00$cphMain$SapGrid\',\'\')">ssssssss</a>';
            ThisCellNewData = "<a class='" + cssClass + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + ThisTableID + "'  onclick='SGV_PostBackClick(this)'>" + cellData + "</a>";
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
    var ThisRowData = SGV_Base64Encode(ThisRowData);
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
            valNumber = SGV_StrToFloat(val);
            tmpFormula = SGV_CustomStrReplace(tmpFormula, key, valNumber, true);
        });
        SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["cumulative"] += eval(tmpFormula);
        ThisCellNewData = SGVGlobalVariables[ContainerId][ThisTableID].columns[CellIndex]["cumulative"];
    } else
        SGV_ErrorMessage("CumulativeSumSourceFieldNotFound");
    return [ThisCellNewData, SGVGlobalVariables, ThisCellNewData];
}

function SGV_HeaderComplex(DataArray, CellName) {
    var res = { title: "", columnsToBeMerged: [] };
    $.each(DataArray.headerComplex, function (k, headerComplexData) {
        var columnsToBeMerged = headerComplexData.columnsToBeMerged;
        var title = headerComplexData.title;
        if (columnsToBeMerged.includes(CellName)) {
            res = { title: title, columnsToBeMerged: columnsToBeMerged };
            return false;
        }
    });
    return res;
}

function SGV_AddFilters(ThisTableID, TbodyID, TheadID, DataArray) {
    var i = 0;
    $.each(DataArray.columns, function (k, column) {

        var TdId = "Footer_" + ThisTableID + "_" + column.data;
        //if (!column.rowGrouping || column.rowGrouping == null || column.rowGrouping.enable == false) {
        if (column.visible == true) {

            var ThisTh = $("#" + TheadID).find(".DT_TrFilters").children("th").eq(i);
            var title = ThisTh.text();
            var width = parseInt(ThisTh.width()) + 20;
            var selectTag = "";
            var inputTag = "";
            if (DataArray.options["dropDownFilterButton"] === true && DataArray.columns[k].dropDownFilter == true) {
                selectTag = "<span class='DT_ColumnFilterContainer'><select style='min-width:" + width + "px;' class='DT_ColumnFilter " + ThisTableID + "ColumnFilter' data-columnnum='" + k + "' data-tbodyid='" + TbodyID + "'><option value=''> " + title + " </option></select></span>";
            }
            if (DataArray.options["columnsSearchButton"] === true) {
                inputTag = "<span class='DT_ColumnSearchContainer'><input style='min-width:" + width + "px;' type='text' class='form-control-sm input-sm DT_ColumnSearch " + ThisTableID + "ColumnSearch' data-columnnum='" + k + "' data-tbodyid='" + TbodyID + "' placeholder='" + title + "'></span>";
            }
            i++;
            ThisTh.html(selectTag + inputTag);
        }
        //}
    });
}

function SGV_AddGeneralSearch(ThisTable, TableInfo, TbodyID, Options) {
    var ThisTableID = TableInfo.TableId;
    var x = $("#" + ThisTableID).closest(".dataTables_wrapper").find(".dt-buttons").children(".DTCustomGeneralSearch").length;
    if (x == 0 && Options["gridSearchTextBox"] === true) {
        $("#" + ThisTableID).closest(".dataTables_wrapper")
            .find(".dt-buttons")
            .prepend("<input type='search' class='form-control form-control-sm DTCustomGeneralSearch " + ThisTableID + "GeneralSearch' placeholder='جستجو در همه ستون ها' aria-controls='" + ThisTableID + "'>")
            .children(".DTCustomGeneralSearch")
            .on("keyup change", function () {
                SGV_SearchCustomized($(this), TbodyID, "GeneralSearch", TableInfo);
            });
    }
    if (ThisTable.closest(".dataTables_scroll").length) {
        var HeaderFiltersThead = ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollHead thead.DT_Thead");
        ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody thead.DT_Thead").removeClass("DT_Thead").addClass("DT_TheadWidthControl");
        ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody tr.DT_TrThead").removeClass("DT_TrThead").addClass("DT_TrWidthControl");
        ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody tfoot").remove();
        ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody .DT_TrFilters").remove();
        //ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollFoot tr.DT_TrTfoot").removeClass("DT_TrTfoot").addClass("DT_TrWidthControl").find("th").html("");
        ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollFoot tr.DT_TrTfoot").removeClass("DT_TrTfoot").addClass("DT_TrWidthControl");
        ThisTable.closest(".dataTables_scroll").find(".dataTables_scrollFoot .DT_TrFilters").removeClass("DT_TrFilters").addClass("DT_TrTfootCalc");
    } else {
        var HeaderFiltersThead = ThisTable.find("thead.DT_Thead");
    }
    return HeaderFiltersThead;
}

function SGV_FillDropDownFilters(ThisTableAPI, TheadID, DataArrayColumns) {
    var sumWidth = 0;
    var i = 0;
    ThisTableAPI.columns().every(function (k) {
        var column = this;
        if (column.visible() == true && (!column.rowGrouping || column.rowGrouping == null || column.rowGrouping.enable == false)) {
            var columnIndex = column.index();
            if (DataArrayColumns[columnIndex].dropDownFilter == true) {
                var thisFixedTh = $("#" + TheadID).find(".DT_TrFilters th").eq(i);
                var thisScrollTh = $(".dataTables_scrollBody thead tr").find("th").eq(i);
                var title = thisFixedTh.text();
                var width = title.length * 10;
                sumWidth += width;
                var tbodyid = thisFixedTh.attr("data-tbodyid");

                thisScrollTh.html("");
                var tmp = [];
                column.data().unique().sort().each(function (d, j) {
                    var d = "<span>" + d + "</span>";
                    var txt = $(d).text();
                    if (txt !== "" && $.inArray(txt, tmp) == -1) {
                        tmp.push(txt);
                        thisFixedTh.find("select").append('<option value="' + txt + '">' + txt + '</option>');
                    }
                });
                var tmp = [];
            }
            i++;
        }
    });
}

function SGV_DefaultOptions(customOptions, GridTitle, ThisTableID, customizeButtons) {
    var ariaControls = "";
    var ExportTitle = "";
    if (customOptions["titleRowInExelExport"] === true) {
        ExportTitle = [null, undefined, NaN, ""].includes(GridTitle) !== true ? SGV_CustomStrReplace(GridTitle, " ", "-") + "-" : "";
        ExportTitle += jalali_today() + "-" + (new Date()).getHours() + ":" + (new Date()).getMinutes() + ":" + (new Date()).getSeconds();
    }
    var DefaultOptions = {
        orderCellsTop: true,
        //colReorder: true,
        searching: true,
        dom: 'lBfrtip',
        stateSave: true,
        stateSaveParams: function (settings, data) {
            delete data.search;
            delete data.columns;
            delete data.columnDefs;
        },
        keys: true,

        //pageLength: 10,
        //info: true,
        //lengthMenu: [[10, 20, 30, 40, 50, 100, -1], [10, 20, 30, 40, 50, 100, "همه"]],

        info: false,
        scrollY: 200,
        scrollX: true,
        scrollCollapse: true,
        paging: false,

        columnDefs: [
            { "orderable": false, "targets": 0 }
        ],

        //fnDrawCallback: function (oSettings) {
        //    var pagination = $(this).closest('.dataTables_wrapper').find('.dataTables_paginate');
        //    pagination.toggle(this.api().page.info().pages > 1);
        //},

        buttons: [],
        language: {
            "thousands": ",",
            "emptyTable": "اطلاعاتی وجود ندارد",
            "info": " _START_ تا _END_ از _TOTAL_ ",
            "infoEmpty": "نمایش 0 تا 0 از 0 موجود",
            "infoFiltered": "(فیلتر از _MAX_ کل موجود)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": " تعداد _MENU_",
            "loadingRecords": "در حال بازیابی...",
            "processing": "در حال بارگزاری...",
            "search": "",
            "searchPlaceholder": "جستجو در همه ستون ها",
            "zeroRecords": "رکوردی پیدا نشد",
            "paginate": {
                "first": "<i class='fa fa-angle-double-left'></i>",
                "last": "<i class='fa fa-angle-double-right'></i>",
                "next": "<i class='fa fa-chevron-left'></i>",
                "previous": "<i class='fa fa-chevron-right'></i>"
            },
            "buttons": {
                "copyTitle": 'کل جدول کپی شد',
                "info": "",
                "copy": "کپی کل جدول",
                "colvis": "فیلتر ستون ها",
                "print": "پرینت"
            },
            "aria": {
                "sortAscending": ": فعال کردن مرتب سازی صعودی ",
                "sortDescending": ": فعال کردن مرتب سازی نزولی "
            }
        }
    };

    var CopyButton = {
        extend: 'copy',
        titleAttr: "کپی کل جدول",
        title: "",
        text: '<i class="fa fa-copy DataTableIcons"></i>',
        footer: true,
        exportOptions: {
            columns: ':visible'
        }
    };
    var PrintButton = {
        extend: 'print',
        titleAttr: "پرینت",
        text: '<i class="fa fa-print DataTableIcons"></i>',
        footer: true,
        exportOptions: {
            columns: ':visible'
        }
    };
    var ExcelButton = {
        extend: 'excel',
        text: '<i class="fa fa-file-excel-o DataTableIcons"></i>',
        titleAttr: "خروجی اکسل",
        title: ExportTitle,
        footer: true,
        exportOptions: {
            columns: ':visible'
        }
    };
    var ColumnsSearchButton = {
        text: '<i class="fa fa-search DataTableIcons DatatableHeaderFilters"></i>',
        titleAttr: "جستجو در هر ستون",
        action: function (e, dt, node, conf) {
            SGV_DatatableHeaderFilters($(this)[0]["node"]["attributes"]["aria-controls"]["value"], "Search");
        }
    };
    var DropDownFilterButton = {
        text: '<i class="fa fa-filter DataTableIcons DatatableHeaderFilters"></i>',
        titleAttr: "فیلتر روی هر ستون",
        action: function (e, dt, node, conf) {
            SGV_DatatableHeaderFilters($(this)[0]["node"]["attributes"]["aria-controls"]["value"], "Filter");
        }
    };
    var RecycleButton = {
        text: '<i class="fa fa-recycle DataTableIcons"></i>',
        titleAttr: "حذف وضعیت های ذخیره شده",
        action: function (e, dt, node, conf) {
            dt.state.clear();
        }
    };
    var RemoveAllFilters = {
        text: '<i class="fa fa-remove DataTableSmallIcons"></i><i class="fa fa-search DataTableIcons"></i>',
        titleAttr: "حذف همه فیلترها و جستجوها",
        action: function (e, dt, node, conf) {
            SGV_ClearAllFilters(dt, ThisTableID);
        }
    };


    //-Start---customizeButtons-------
    if (customizeButtons) {
        for (const [k, btnArray] of Object.entries(customizeButtons)) {
            if (btnArray.javascriptMethodName == null || btnArray.javascriptMethodName.trim() == "") {
                SGV_ErrorMessage("CustomizeButtonJsUnDefine", btnArray.javascriptMethodName);
            } else if (btnArray.buttonName == null || btnArray.buttonName == 0) {
                SGV_ErrorMessage("CustomizeButtonNameUnDefine", btnArray.buttonName);
            } else {
                customButton(btnArray);
            }
        }
    }
    function customButton(btnArray) {
        var fn = window[btnArray.javascriptMethodName];
        if (typeof fn === "function") {
            var btnOptions = fn.apply(window, [{ YourData: btnArray.data }]);

            if (btnArray.buttonName == 1) { //Copy
                CopyButton = btnOptions;
            } else if (btnArray.buttonName == 2) { //Print
                PrintButton = btnOptions;
            } else if (btnArray.buttonName == 3) { //Excel
                ExcelButton = btnOptions;
            } else if (btnArray.buttonName == 4) { //ColumnsSearch
                ColumnsSearchButton = btnOptions;
            } else if (btnArray.buttonName == 5) { //Recycle
                DropDownFilterButton = btnOptions;
            } else if (btnArray.buttonName == 6) { //DropDownFilter
                RecycleButton = btnOptions;
            }

        } else {
            SGV_ErrorMessage("CustomizeButtonJsFunNotFound", btnArray.javascriptMethodName);
        }
    }
    //-End---customizeButtons-------

    if (customOptions["copyButton"] === true) DefaultOptions.buttons.push(CopyButton);
    if (customOptions["printButton"] === true) DefaultOptions.buttons.push(PrintButton);
    if (customOptions["excelButton"] === true) DefaultOptions.buttons.push(ExcelButton);
    if (customOptions["columnsSearchButton"] === true) DefaultOptions.buttons.push(ColumnsSearchButton);
    if (customOptions["dropDownFilterButton"] === true) DefaultOptions.buttons.push(DropDownFilterButton);
    if (customOptions["recycleButton"] === true) DefaultOptions.buttons.push(RecycleButton);
    if (customOptions["removeAllFilters"] === true) DefaultOptions.buttons.push(RemoveAllFilters);
    $.each(customOptions, function (k, v) {
        if (["lengthMenu", "order"].includes(k) && Array.isArray(DefaultOptions[k]) !== true) {
            v = v !== null ? JSON.parse(v.replace(/'/gi, "\"")) : [];
            DefaultOptions[k] = v;
        } else {
            DefaultOptions[k] = v;
        }
    });
    return DefaultOptions;
}

function SGV_CounterColumn(TableObject) {
    var i = 1;
    TableObject.rows({ order: 'applied' }).every(function (rowIdx, tableLoop, rowLoop) {
        TableObject.cell(rowIdx, 0).data(i++);
    });
}

function SGV_DTOrderChage(DTOrderChangeFunctions, ThisColumnDefs, TableInfo, CounterColumn) {
    TableInfo.TableObject.on("draw", function (changeEvent) {
        if (CounterColumn == true)
            SGV_CounterColumn(TableInfo.TableObject);
        if (DTOrderChangeFunctions.length > 0) {
            $.each(DTOrderChangeFunctions, function (c, DTOrderChange) {
                var CellIndex = TableInfo.SGVGlobalVariables[TableInfo.ContainerId][TableInfo.TableId]["columnsName"][DTOrderChange.CellName];
                if (DTOrderChange) {
                    $.each(DTOrderChange["CellFuncArray"], function (kf, CellFunc) {
                        if (CellFunc) {
                            var fn = window["SGV_" + CellFunc.funcName + "_AfterDraw"]; //SGV_CumulativeSum_AfterDraw
                            if (typeof fn === "function") {
                                fn.apply(window, [CellIndex, CellFunc, DTOrderChangeFunctions, ThisColumnDefs, TableInfo]);
                            }
                        }
                    });
                }
            });
        }
    }).draw();
}

function SGV_CumulativeSum_AfterDraw(CellIndex, CellFunc, DTOrderChangeFunctions, ThisColumnDefs, TableInfo) {
    var sum = 0;
    TableInfo.TableObject.column(CellIndex).rows({ order: 'applied', filter: 'applied', search: 'applied' }).indexes().each(function (rowIndex, i, allFilteredIndexes) {
        var tmpFormula = CellFunc.sourceField;
        $.each(TableInfo.Columns, function (cIndex, cArray) {
            var cName = cArray.data;
            var cValue = TableInfo.TableObject.cell(rowIndex, cIndex).data();
            cValue = SGV_StrToFloat(cValue);
            tmpFormula = SGV_CustomStrReplace(tmpFormula, cName, cValue, true);
        });
        var v = eval(tmpFormula);
        sum += v;
        TableInfo.TableObject.cell(rowIndex, CellIndex).data(sum.toLocaleString(undefined, { maximumFractionDigits: 3 }));
    });
}

function SGV_DTCreatedCell(DTCreatedCellFunctions, ThisColumnDefs, SGVGlobalVariables, ThisTableID, ContainerId) {
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
                                    var ThisTDNewData = tmp[2];

                                    $(td).html(ThisTDNewData);
                                }

                            }
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

function SGV_PostBackClick(obj) {
    //__doPostBack('ctl00$cphMain$am', '');
    //__doPostBack('ctl00$cphMain$SapGrid', '');
    __doPostBack('SapGrid', '');
    //javascript: __doPostBack('ctl00$cphMain$SapGridPostBack', '');
}

function SGV_CallJavaScriptMethodClick(obj) {
    var CallBackData = SGV_Base64Decode(obj.dataset.row);
    var CallBackData = JSON.parse(CallBackData);
    var ThisTableID = obj.dataset.tableid;
    var JavaScriptMethodName = obj.dataset.javascriptmethod;
    var gridParameters = $("#" + ThisTableID).attr("data-gridparameters");
    var gridParameters = SGV_Base64Decode(gridParameters);
    CallBackData["GridParameters"] = JSON.parse(gridParameters);
    var fn = window[JavaScriptMethodName];
    if (typeof fn === "function") {
        fn.apply(window, [{ TableID: ThisTableID, CallBackData: CallBackData, ClickedObject: obj }]);
    }
}

function SGV_AjaxClick(obj) {
    var objText = obj.text ? obj.text : "untitled";
    $(".SGV_LoadingContainer").show();
    var CallBackData = SGV_Base64Decode(obj.dataset.row);
    var CallBackData = JSON.parse(CallBackData);
    var ThisTableID = obj.dataset.tableid;
    var ContainerId = obj.dataset.containerid;
    var cellName = obj.dataset.cellname;
    var ThisWebMethodName = obj.dataset.webmethodname;
    var GridFirstText = [null, undefined, NaN, ""].includes(obj.dataset.nexttabtitle) === false ? obj.dataset.nexttabtitle : GridFirstText;
    GridFirstText = SGV_CustomStrReplace(GridFirstText, "{clickedItem}", objText, false, true);
    $.each(CallBackData.RowData, function (key, val) {
        GridFirstText = SGV_CustomStrReplace(GridFirstText, key, val);
    });
    var gridParameters = $("#" + ThisTableID).attr("data-gridparameters");
    var gridParameters = SGV_Base64Decode(gridParameters);
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
                SapGridViewJSBind(d, JSON.parse(CallBackData).FuncArray.Level, GridFirstText);
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

function SGV_AfterFilter_TheadTfootCalc(TableInfo) {
    var ThisFooter = $("#" + TableInfo["TableId"]).closest(".dataTables_wrapper").find(".dataTables_scrollFoot");
    ThisFooter.find(".DT_TrTfootCalc").find("th").html("");
    var showFooter = false;
    var ContainerId = TableInfo.ContainerId;
    var ColumnNumberIncludingStatus = 0;
    $.each(TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns, function (CellIndex, cell) {
        if (TableInfo.Columns[CellIndex].visible === true) {
            var ThisValue = cell.footerValue;
            var ThisDisplayValue = "";
            var ThisVal_OpenTag = "";
            var ThisVal_CloseTag = "";
            var ItemCss = TableInfo.Columns[CellIndex].className;
            var cellName = cell.name;
            var tdTitle = "";
            $.each(TableInfo.Columns[CellIndex].functions, function (k, FuncArray) {
                if ([2].includes(parseInt(FuncArray.section))) {
                    if (FuncArray.funcName == "Calc" && FuncArray.formula == null && FuncArray.operator == 0) {

                        // Total over all pages
                        var total = TableInfo.TableAPI
                            .column(CellIndex)
                            .data()
                            .reduce(function (a, b) {
                                return SGV_StrToFloat(a) + SGV_StrToFloat(b);
                            }, 0);

                        // Total over this page
                        var pageTotal = TableInfo.TableAPI
                            .column(CellIndex, { page: 'current' })
                            .data()
                            .reduce(function (a, b) {
                                return SGV_StrToFloat(a) + SGV_StrToFloat(b);
                            }, 0);

                        // Total over filter
                        var pageFilter = TableInfo.TableAPI
                            .column(CellIndex, { filter: 'applied' })
                            .data()
                            .reduce(function (a, b) {
                                return SGV_StrToFloat(a) + SGV_StrToFloat(b);
                            }, 0);


                        tdTitle += "جمع این صفحه: ";
                        tdTitle += pageTotal.toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                        tdTitle += "\nجمع فیلتر شده ها: ";
                        tdTitle += pageFilter.toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                        tdTitle += "\nجمع کل: ";
                        tdTitle += total.toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                        ThisValue = pageFilter;
                        //$(TableInfo.TableAPI.column(CellIndex).footer()).html('$' + pageTotal + ' ( $' + total + ' total)');
                    }
                    else if (FuncArray.funcName == "Separator" && ThisValue !== undefined) {
                        ThisDisplayValue = SGV_StrToFloat(ThisValue).toLocaleString(FuncArray.locales, { minimumFractionDigits: FuncArray.minimumFractionDigits, maximumFractionDigits: FuncArray.maximumFractionDigits });
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
                            condition = SGV_CustomStrReplace(condition, columnName, footerValue, true);
                            footerValue = columnName == cellName && ThisDisplayValue !== "" ? ThisDisplayValue : footerValue;
                            isTrueText = SGV_CustomStrReplace(isTrueText, columnName, footerValue, true);
                            isFalseText = SGV_CustomStrReplace(isFalseText, columnName, footerValue, true);
                        });
                        if (eval(condition)) {
                            $.each(strReplace, function (key, val) {
                                isTrueText = SGV_CustomStrReplace(isTrueText, key, val);
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
            var TdId = "Footer_" + TableInfo.TableId + "_" + cell.name; //dataTables_scrollFoot
            if (["undefined", undefined, "NaN", NaN].includes(TempThisVal) === false) {
                $(".dataTables_scrollFoot #" + TdId).html(TempThisVal);
                ThisFooter.find(".DT_TrWidthControl").children("th." + cell.name + "_Class").html(TempThisVal);
                $(".dataTables_scrollFoot #" + TdId).attr("title", tdTitle);
                ThisFooter.find(".DT_TrWidthControl").children("th." + cell.name + "_Class").attr("title", tdTitle);
                //ThisFooter.find(".DT_TrTfootCalc th").eq(CellIndex).html(TempThisVal); after a column to be false visible, not Work
            }
            if (ItemCss != "") {
                $(".dataTables_scrollFoot #" + TdId).addClass(ItemCss);
                //ThisFooter.find(".DT_TrTfootCalc th").eq(CellIndex).addClass(ItemCss); after a column to be false visible, not Work
            }
            showFooter = true;
            ColumnNumberIncludingStatus++;
        }
    });
    /*if (showFooter)
        ThisFooter.show();*/
}

function SGV_TheadTfootCalc(TableInfo) {
    var ThisFooter = $("#" + TableInfo["TableId"]).closest(".dataTables_wrapper").find(".dataTables_scrollFoot");
    ThisFooter.find(".DT_TrTfootCalc").find("th").html("");
    var showFooter = false;
    var ContainerId = TableInfo.ContainerId;
    var ColumnNumberIncludingStatus = 0;
    $.each(TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns, function (CellIndex, cell) {
        if (TableInfo.Columns[CellIndex].visible === true) {
            var ThisValue = cell.footerValue;
            var ThisDisplayValue = "";
            var ThisVal_OpenTag = "";
            var ThisVal_CloseTag = "";
            var ItemCss = TableInfo.Columns[CellIndex].className;
            var cellName = cell.name;
            var TdId = "Footer_" + TableInfo.TableId + "_" + cell.name;
            $.each(TableInfo.Columns[CellIndex].functions, function (k, FuncArray) {
                if ([2].includes(parseInt(FuncArray.section))) {

                    if (FuncArray.funcName == "OnClick") {
                        var rowAllData = {};
                        rowAllData["FuncArray"] = FuncArray;
                        rowAllData["RowData"] = {};
                        var ThisRowData = JSON.stringify(rowAllData);
                        var ThisRowData = SGV_Base64Encode(ThisRowData);
                        var cssClass = FuncArray.cssClass ? FuncArray.cssClass : "btn btn-link text-danger p-0 m-0";
                        var cellText = FuncArray.footerText != null ? FuncArray.footerText : ThisValue;
                        var webMethodName = FuncArray.webMethodName ? FuncArray.webMethodName : "SapGridEvent";
                        var nextTabTitle = FuncArray.nextTabTitle ? FuncArray.nextTabTitle : "";
                        ThisValue = cellText;
                        if (FuncArray.enable == true) {
                            ThisVal_OpenTag += "<a class='" + cssClass + "' data-nexttabtitle='" + nextTabTitle + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + TableInfo["TableId"] + "' onclick='SGV_AjaxClick(this)'>"
                            ThisValue = cellText;
                            ThisVal_CloseTag += "</a>";
                        }
                    }
                    else if (FuncArray.funcName == "Calc" && FuncArray.formula != null) {
                        var tmpFormula = FuncArray.formula;
                        $.each(TableInfo.SGVGlobalVariables[ContainerId][TableInfo["TableId"]].columns, function (key, val) {
                            if ([null, undefined, NaN, 'undefined', 'NaN', 'null'].includes(val["footerValue"]) == false && [null, undefined, NaN, 'undefined', 'NaN', 'null'].includes(val["name"]) == false) {
                                var footerValue = val["footerValue"];
                                var columnName = val["name"];
                                footerValue = SGV_CustomStrReplace(footerValue.toString(), ",", "");
                                tmpFormula = SGV_CustomStrReplace(tmpFormula, columnName, footerValue, true);
                            }
                        });
                        try {
                            ThisValue = eval(tmpFormula);
                        } catch (e) {
                            if (e instanceof SyntaxError) {
                                console.log(e.message);
                                console.log("-------------------------");
                            }
                        }
                    }

                    else if (FuncArray.funcName == "Separator" && ThisValue !== undefined) {
                        ThisDisplayValue = SGV_StrToFloat(ThisValue).toLocaleString(FuncArray.locales, { minimumFractionDigits: FuncArray.minimumFractionDigits, maximumFractionDigits: FuncArray.maximumFractionDigits });
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
                            condition = SGV_CustomStrReplace(condition, columnName, footerValue, true);
                            footerValue = columnName == cellName && ThisDisplayValue !== "" ? ThisDisplayValue : footerValue;
                            isTrueText = SGV_CustomStrReplace(isTrueText, columnName, footerValue, true);
                            isFalseText = SGV_CustomStrReplace(isFalseText, columnName, footerValue, true);
                        });
                        if (eval(condition)) {
                            $.each(strReplace, function (key, val) {
                                isTrueText = SGV_CustomStrReplace(isTrueText, key, val);
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
            if (["undefined", undefined, "NaN", NaN].includes(TempThisVal) === false) {
                $("#" + TdId).html(TempThisVal);
                ThisFooter.find(".DT_TrWidthControl").children("th." + cell.name + "_Class").html(TempThisVal);
                //ThisFooter.find(".DT_TrTfootCalc th").eq(CellIndex).html(TempThisVal); not Work after a column to be visible false
            }
            if (ItemCss != "") {
                $("#" + TdId).addClass(ItemCss);
                //ThisFooter.find(".DT_TrTfootCalc th").eq(CellIndex).addClass(ItemCss); not Work after a column to be visible false
            }
            showFooter = true;
            ColumnNumberIncludingStatus++;
        }
    });
    if (showFooter)
        ThisFooter.show();
}

function SGV_TabsControl(ThisTabID, ThisTabContentID, TabsContainerID, Level, ThisTabTitle, ContainerId) {
    if ($("#" + ThisTabID).length > 0) {
        ThisTab = "Exist";
    } else {
        var ThisTab = "<div class='SGV_Tab SGV_ActiveTabTitle " + ContainerId + "Tab' id='" + ThisTabID + "' onclick='SGV_TabOnClick(this)' data-containerid='" + ContainerId + "' data-tabid='" + ThisTabID + "' data-contentid='" + ThisTabContentID + "' > ";
        ThisTab += ThisTabTitle;
        ThisTab += " </div> ";
        //ThisTab += "<i class='fa fa-remove SGV_CloseTab' onclick='SGV_CloseTab(this);' data-tabid='" + ThisTabID + "' data-contentid='" + ThisTabContentID + "'></i>";
    }
    if (parseInt(Level) > 1) {
        SGV_TabSwitch(ThisTabID, ThisTabContentID, ContainerId);
        $("#" + TabsContainerID).show();
    }
    return ThisTab;
}

function SGV_CloseTab(obj) {
    var ThisTabID = obj.dataset.tabid;
    var ThisTabContentID = obj.dataset.contentid;
    $("#" + ThisTabID).hide(500);
    $("#" + ThisTabContentID).hide(500);
    //$("#" + ThisTabContentID).hide(500);

    /*$("#" + ThisTabID).closest(".SGV_TabsContainer").find(".SGV_Tab").eq(0).addClass("SGV_ActiveTabTitle");
    $("#" + ThisTabContentID).parent().find(".SGV_GridContent").eq(0).addClass("SGV_ActiveTabContent");*/
}

function SGV_TabOnClick(obj) {
    var ThisTabID = obj.dataset.tabid;
    var ThisTabContentID = obj.dataset.contentid;
    var ContainerId = obj.dataset.containerid;
    SGV_TabSwitch(ThisTabID, ThisTabContentID, ContainerId);
}

function SGV_TabSwitch(ThisTabID, ThisTabContentID, ContainerId) {
    $("." + ContainerId + "Tab").removeClass("SGV_ActiveTabTitle");
    $("." + ContainerId + "GridContent").removeClass("SGV_ActiveTabContent");
    $("#" + ThisTabID).addClass("SGV_ActiveTabTitle").css("display", "");
    $("#" + ThisTabContentID).addClass("SGV_ActiveTabContent").css("display", "");
}

function SGV_OnChangeFilters(TableInfo) {
    $(".DT_ColumnFilter").on('change', function () {
        var allInput = $(this).closest("tr").find("select");
        var tbodyid = $(this).attr("data-tbodyid");
        SGV_SearchCustomized(allInput, tbodyid, "ColumnFilter", TableInfo);
    });
    $(".DT_ColumnSearch").on('keyup change', function () {
        var allInput = $(this).closest("tr").find("input");
        var tbodyid = $(this).attr("data-tbodyid");
        SGV_SearchCustomized(allInput, tbodyid, "ColumnSearch", TableInfo);
    });
    $(".dataTables_filter input").on('keyup', function () {
        var allInput = $(this);
        var tbodyid = $(this).closest(".dataTables_wrapper").find("th").attr("data-tbodyid");
        SGV_SearchCustomized(allInput, tbodyid, "GeneralSearch", TableInfo);
    });
}

function SGV_HeightControl(ThisTable, gridheight) {
    ThisTable.closest('.dataTables_scrollBody').each(function () {
        var ThisGridRows = $(this);
        ThisGridRows[0].style.setProperty('max-height', gridheight + 'px', 'important');

        /* -- KeepScrolHeight
        var id = ThisGridRows.find("table.dataTable").attr("id");
        scrollArray = (typeof scrollArray != 'undefined' && scrollArray instanceof Array) ? scrollArray : [];
        if (typeof (scrollArray[id]) !== 'undefined') {
            var thisGridScrollValue = scrollArray[id];
        } else {
            var thisGridScrollValue = 0;
        }
        ThisGridRows.scroll(function () {
            if (ThisGridRows.html().length) {
                scrollArray[id] = ThisGridRows.scrollTop();
            }
        });
        ThisGridRows.scrollTop(thisGridScrollValue);
        */
    });
}

function SGV_ClearAllFilters(TableObject, ThisTableID) {
    $("." + ThisTableID + "GeneralSearch").val("");
    $("." + ThisTableID + "ColumnSearch").val("");
    $("." + ThisTableID + "ColumnFilter").prop("selectedIndex", 0);
    TableObject.search('').columns().search('').draw();
}

function SGV_SearchCustomized(allInput, tbodyid, SearchType, TableInfo) {
    var emptyArray = [null, "", NaN, undefined];
    var dataSearch = {};
    var i = 0;
    var TableObject = TableInfo.TableObject;
    var ThisTableID = TableInfo.TableID;
    TableObject.search('').columns().search('').draw(); //Clear All Search & Filters
    var elementsType = "";
    allInput.each(function () {
        var inputData = $(this).val();
        //let inputData = SGV_ArabicToPersianChar(inputData); نباید تبدیل به فارسی شود، چراکه جایی
        var columnNum = $(this).attr("data-columnnum");
        var elementType = $(this).prop("nodeName");
        if (inputData.trim() != "" && inputData != "undefined") {
            dataSearch[i] = { "inputData": inputData, "columnNum": columnNum, "tbodyid": tbodyid, "elementType": elementType };
            elementsType = elementType.toUpperCase();
            i++;
        }
    });

    if (elementsType == "SELECT") {
        $("." + ThisTableID + "GeneralSearch").val("");
        $("." + ThisTableID + "ColumnSearch").val("");
    } else if (elementsType == "INPUT" && SearchType == "GeneralSearch") {
        $("." + ThisTableID + "ColumnFilter").prop("selectedIndex", 0);
        $("." + ThisTableID + "ColumnSearch").val("");
    } else if (elementsType == "INPUT" && SearchType == "ColumnSearch") {
        $("." + ThisTableID + "ColumnFilter").prop("selectedIndex", 0);
        $("." + ThisTableID + "GeneralSearch").val("");
    }

    $.each(dataSearch, function (k, v) {
        if (emptyArray.includes(v.inputData) !== true) {
            let persianText = SGV_ArabicToPersianChar(v.inputData);
            let arabicText = SGV_PersianToArabicChar(v.inputData);
            if (SearchType == "GeneralSearch") {
                TableObject.search(persianText + '|' + arabicText, true, false).draw(); //smart search
            }
            else if (SearchType == "ColumnSearch") {
                TableObject.columns(v.columnNum).search(persianText + '|' + arabicText, true, false).draw(); //smart search
            }
            else {
                TableObject.columns(v.columnNum).search(v.inputData ? '^' + v.inputData + '$' : '', true, false).draw(); //match search for dropdown filter
            }
        }
    });
}

//-SapGridView-Tools--------------------------------------------------------------------------

function SGV_StrToFloat(str) {
    var str = str !== undefined && str != null && str != NaN && $("<span>" + str + "</span>").text().trim() != "" ? $("<span>" + str + "</span>").text().trim() : 0;
    str = SGV_CustomStrReplace(str.toString(), "/", ".");
    str = str.replace(/[^\d.-]/g, '');
    return parseFloat(str);
}

function SGV_CustomStrReplace(str, searchValue, replaceValue, matchWholeWord, findAllAndReplace) {
    var findAllAndReplace = findAllAndReplace ? findAllAndReplace : true;
    str = (str + " ").trim();
    if (findAllAndReplace) {
        if ([null, NaN, undefined, 'null', 'NaN', 'undefined', ""].includes(str) === false) {
            if (matchWholeWord)
                str = str.replace(new RegExp("\\b" + searchValue + "\\b", "g"), replaceValue);
            else
                str = str.replace((new RegExp(searchValue, "g")), replaceValue);
        }
    } else {
        str = str.replace(searchValue, replaceValue);
    }
    return str;
}

function SGV_IsValidDate(dateObject) {
    return new Date(dateObject).toString() !== 'Invalid Date';
}

function SGV_DatatableHeaderFilters(TableId, type) {
    var ThisDataTable = $("#" + TableId).closest(".dataTables_wrapper");
    var ThisThead = ThisDataTable.find(".DT_TrFilters");
    var AllTheadFiltersID = ThisDataTable.find(".DT_TrFilters").find(".DT_ColumnFilterContainer, .DT_ColumnSearchContainer");
    var ThisTheadFilterID = ThisDataTable.find(".DT_TrFilters").find(".DT_Column" + type + "Container");
    var flag = ThisThead.attr("data-flag");
    if (flag == "hide" || flag != type) {
        ThisThead.attr("data-flag", type);
        ThisThead.slideDown();
        AllTheadFiltersID.hide();
        ThisTheadFilterID.fadeIn();
    } else {
        ThisThead.attr("data-flag", "hide");
        ThisThead.hide();
        AllTheadFiltersID.slideUp();
    }
};

function SGV_Base64Decode(str) {
    // Unicode support
    return decodeURIComponent(atob(str).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
}

function SGV_Base64Encode(str) {
    // Unicode support
    return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
        function toSolidBytes(match, p1) {
            return String.fromCharCode('0x' + p1);
        }));
}

function SGV_ArabicToPersianChar(str) {
    //var c = str.replace(/ى/g,"ی");
    var c = str.replace(/ي/g, "ی");
    var c = c.replace(/ئ/g, "ی");
    var c = c.replace(/ة/g, "ه");
    var c = c.replace(/ك/g, "ک");
    var c = c.replace(/ؤ/g, "و");
    return c;
}

function SGV_PersianToArabicChar(str) {
    //var c = str.replace(/ى/g,"ي");
    //var c = str.replace(/ى/g,"ی");
    var c = str.replace(/ی/g, "ي");
    var c = c.replace(/ک/g, "ك");
    return c;
}

function SGV_ErrorMessage(errorKey, otherInfo = null) {
    var errorArray = {
        CumulativeSumSourceFieldNotFound: "در متد جمع انباشته ستونی برای مقدار اولیه انتخاب نشده",
        CumulativeSumSourceFieldNotFoundInRowData: "در متد جمع انباشته ستونی که برای مقدار اولیه انتخاب شده در اطلاعات سطر وجود ندارد",
        VerticalSumWithoutSelectFooterSection: "در گرید جمع سطرهای یک ستون انتخاب شده ولی قسمت فوتر انتخاب نشده",
        TextFeatureConditionNotFound: "در گرید تغییر متن فیلد خواسته شده ولی شرطی وجود ندارد",
        CustomizeButtonJsFunNotFound: "در قسمت سفارشی سازی دکمه های گرید یک متد جاوااسکریپت با یک نام تعریف شده که در صفحه شما وجود ندارد",
        CustomizeButtonJsUnDefine: "در قسمت سفارشی سازی دکمه های گرید متد جاوااسکریپت بدون مقدار تعریف شده",
        CustomizeButtonNameUnDefine: "در قسمت سفارشی سازی دکمه های گرید نام دکمه بدون مقدار تعریف شده"
    };
    if (otherInfo)
        console.log(errorArray[errorKey], otherInfo);
    else
        console.log(errorArray[errorKey]);
}

class charts {
    tableObject = null;
    titleAlign = { 0: "right", 1: "center", 2: "left" };
    chartsData = null;
    mainColumnsTitle = null;

    constructor(m) {
        this.tableObject = m.tableObject;
        this.mainColumnsTitle = m.mainColumnsTitle;

        //One-time trace data and charts-data assignment
        this.setChartsData(m.grid.charts);

        //Making requested charts
        this.traceCharts(m.grid.charts);

        $(".highcharts-credits").hide();
    }

    //#region Prepare data for charts
    setChartsData(charts, self = this) {
        let chartsData = {};
        self.tableObject.rows({ order: 'applied', filter: 'applied', search: 'applied' }).every(function (rowIdx, tableLoop, rowLoop) {
            let rowData = this.data();
            $.each(charts, function (k, chart) {
                let chartName = self.getChartType(chart.chartName);

                let fName = "set" + chartName + "Data";
                if (typeof self[fName] === "function") {
                    chartsData = self[fName](chartsData, rowData, chart, chartName); //Example: setpieData
                }
            });
        });
        self.chartsData = chartsData;
    }

    setpieData(chartsData, rowData, chart, chartName, self = this) {
        if (!chartsData[chartName]) {
            chartsData[chartName] = {};
            chartsData[chartName]["data"] = [];
        }
        chartsData[chartName].data.push({ name: rowData[chart.key], y: rowData[chart.value] });
        return chartsData;
    }

    setcolumnData(chartsData, rowData, chart, chartName, self = this) {
        if (!chartsData[chartName]) {
            chartsData[chartName] = {};
            chartsData[chartName]["categories"] = [];
            chartsData[chartName]["series"] = [];
            $.each(chart.series, function (k, sery) {
                chartsData[chartName]["series"].push({ name: self.mainColumnsTitle[sery], customKey: sery, data: [] });
            });
        }
        $.each(chartsData[chartName]["series"], function (k, sery) {
            chartsData[chartName]["series"][k].data.push(rowData[sery.customKey]);
        });
        chartsData[chartName]["categories"].push(rowData[chart.xAxis.categories]);
        return chartsData;
    }

    setlineData(chartsData, rowData, chart, chartName, self = this) {
        if (!chartsData[chartName]) {
            chartsData[chartName] = {};
            chartsData[chartName]["categories"] = [];
            chartsData[chartName]["series"] = [];
            $.each(chart.series, function (k, sery) {
                chartsData[chartName]["series"].push({ name: self.mainColumnsTitle[sery], customKey: sery, data: [] });
            });
        }
        $.each(chartsData[chartName]["series"], function (k, sery) {
            chartsData[chartName]["series"][k].data.push(rowData[sery.customKey]);
        });
        chartsData[chartName]["categories"].push(rowData[chart.xAxis.categories]);
        return chartsData;
    }
    //#endregion

    //#region call charts
    traceCharts(charts, self = this) {
        $.each(charts, function (k, chart) {
            let chartName = self.getChartType(chart.chartName);
            if (typeof self[chartName] === "function") {
                self[chartName](chart);
            }
        });
    }
    //#endregion

    //#region charts methods
    pie(chart, self = this) {
        let data = self.chartsData && self.chartsData.pie && self.chartsData.pie.data ? self.chartsData.pie.data : [];
        Highcharts.chart(chart.chartContainerId, {
            chart: {
                type: self.getChartType(chart.chartName),
                styledMode: true
            },
            title: {
                text: chart.title && chart.title.text ? chart.title.text : "",
                align: self.titleAlign[chart.title.align] ? self.titleAlign[chart.title.align] : "center"
            },
            subtitle: {
                text: chart.subTitle && chart.subTitle.text ? chart.subTitle.text : "",
                align: chart.subTitle && self.titleAlign[chart.subTitle.align] ? self.titleAlign[chart.subTitle.align] : "center"
            },
            /*tooltip: {
                formatter: function () {
                    let Tooltip = (this.y) + "___" + this.point.name;
                    return Tooltip;
                }
            },*/
            series: [
                {
                    name: chart.seriesName,
                    data: data
                }
            ]
        });
    }

    column(chart, self = this) {
        let categories = self.chartsData && self.chartsData.column && self.chartsData.column.categories ? self.chartsData.column.categories : [];
        let series = self.chartsData && self.chartsData.column && self.chartsData.column.series ? self.chartsData.column.series : [];
        Highcharts.chart(chart.chartContainerId, {
            chart: {
                type: self.getChartType(chart.chartName)
            },
            title: {
                text: chart.title && chart.title.text ? chart.title.text : "",
                align: self.titleAlign[chart.title.align] ? self.titleAlign[chart.title.align] : "center"
            },
            subtitle: {
                text: chart.subTitle && chart.subTitle.text ? chart.subTitle.text : "",
                align: chart.subTitle && self.titleAlign[chart.subTitle.align] ? self.titleAlign[chart.subTitle.align] : "center"
            },
            xAxis: {
                categories: categories,
                crosshair: true,
                accessibility: {
                    description: chart.xAxis.accessibility
                },
                title: {
                    text: chart.xAxis.title && chart.xAxis.title.text ? chart.xAxis.title.text : "",
                    //align in xAxix not work
                    //align: self.titleAlign[chart.xAxis.title.align] ? self.titleAlign[chart.xAxis.title.align] : "center"
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: chart.yAxis.title && chart.yAxis.title.text ? chart.yAxis.title.text : "",
                    //align in yAxis not work
                    //align: self.titleAlign[chart.yAxis.title.align] ? self.titleAlign[chart.yAxis.title.align] : "center"
                }
            },
            tooltip: {
                valueSuffix: chart.tooltip && chart.tooltip.valueSuffix ? chart.tooltip.valueSuffix : ""
            },
            plotOptions: {
                column: {
                    pointPadding: chart.plotOptions.column.pointPadding,
                    borderWidth: chart.plotOptions.column.borderWidth
                }
            },
            series: series
        });
    }

    line(chart, self = this) {
        let categories = self.chartsData && self.chartsData.line && self.chartsData.line.categories ? self.chartsData.line.categories : [];
        let series = self.chartsData && self.chartsData.line && self.chartsData.line.series ? self.chartsData.line.series : [];
        Highcharts.chart(chart.chartContainerId, {
            chart: {
                type: self.getChartType(chart.chartName)
            },
            title: {
                text: chart.title && chart.title.text ? chart.title.text : "",
                align: self.titleAlign[chart.title.align] ? self.titleAlign[chart.title.align] : "center"
            },
            subtitle: {
                text: chart.subTitle && chart.subTitle.text ? chart.subTitle.text : "",
                align: chart.subTitle && self.titleAlign[chart.subTitle.align] ? self.titleAlign[chart.subTitle.align] : "center"
            },
            xAxis: {
                categories: categories,
            },
            yAxis: {
                title: {
                    text: chart.yAxis.title && chart.yAxis.title.text ? chart.yAxis.title.text : "",
                    //align in yAxis not work
                    //align: self.titleAlign[chart.yAxis.title.align] ? self.titleAlign[chart.yAxis.title.align] : "center"
                }
            },
            tooltip: {
                valueSuffix: chart.tooltip && chart.tooltip.valueSuffix ? chart.tooltip.valueSuffix : ""
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: chart.plotOptions.line.enabled
                    },
                    enableMouseTracking: chart.plotOptions.line.enableMouseTracking
                }
            },
            series: series
        });
    }
    //#endregion

    //#region tools
    getChartType(t) {
        return t.toLowerCase().replace("chart", "");
    }
    //#endregion
}

/*
 *
 *
 *
function OLDDDDDDDDD____SGV_SearchCustomized(allInput, tbodyid, SearchInTr) {
                var dataSearch = [];
                var i = 0;
                allInput.each(function () {
                    var inputData = $(this).val();
                    let persianInputData = SGV_ArabicToPersianChar(inputData);
                    var columnNum = $(this).attr("data-columnnum");
                    if (persianInputData.trim() != "" && persianInputData != "undefined") {
                        dataSearch[i] = { "inputData": persianInputData, "columnNum": columnNum, "tbodyid": tbodyid };
                        i++;
                    }
                });
                $("#" + tbodyid + " > tr").show();
                $("#" + tbodyid + " > tr").each(function () {
                    var thisTr = $(this).closest("tr");
                    $.each(dataSearch, function (k, v) {
                        if (SearchInTr === 0)
                            var thisSearchField = thisTr.find("td:eq(" + v.columnNum + ")");
                        else
                            var thisSearchField = thisTr;
                        var text = thisSearchField.text();
                        thisSearchField.find("input, select, button, a, span").each(function () {
                            if ($(this).is("button, a, span")) {
                                text += $(this).text();
                            } else
                                text += $(this).val();
                        });
                        let persianText = SGV_ArabicToPersianChar(text);
                        if (persianText.indexOf(v.inputData) < 0) {
                            thisTr.hide();
                        }
                    });
                });
            }
 function EventFired_SGV(TableObject) {
    TableObject.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        cell.innerHTML = i + 1;
    });
}*
---------------------------------------------------------
 *
 * function Options_SGV(DataArray, options) {
    if (options !== null) {
        options["lengthMenu"] = JSON.parse(options["lengthMenu"].replace(/'/gi, "\""));
        $.each(options, function (k, v) {
            DataArray[k] = v;
        });
    }
    return DataArray;
}
 *
 *
---------------------------------------------------------
---------------------------------------------------------
*/