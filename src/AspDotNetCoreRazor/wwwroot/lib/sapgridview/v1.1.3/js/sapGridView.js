class sapGridView {
    model = null;
    //#region Constructor & Properties
    constructor(data, level, gridFirstText) {
        if (data)
            this.handle(data, level, gridFirstText);
    }
    //#endregion

    //#region handle
    handle(data, level, gridFirstText) {
        let self = this;
        data = Array.isArray(data) ? data : JSON.parse(JSON.stringify(data));
        $.each(data["Grids"], function (gridName, grid) {
            grid["data"] = Array.isArray(grid["data"]) == false ? JSON.parse(grid["data"]) : grid["data"];
            self.model = new sapGridViewModel().setModelProps(data, gridName, grid, level, gridFirstText);
            self.gridBind();
        });
    }

    gridBind() {
        this.addRowCounter();
        this.setSGVGlobalVariables();
        this.setSGVArray();
        this.columnsTrace();
        this.removeNullsFromTotalFunctionDetails();
        this.addEmptyTable();
        this.setDefaultOptions();
        this.removeLastSameGrid();
        this.addRowGrouping();
        this.addServerSideProcessing();
        this.exec();
        this.setTableInfo();
        this.onDraw();
        this.addGeneralSearch();
        this.addFilters();
        this.fillDropDownFilters();
        this.onChangeFilters();
        this.keepScrolHeight();
        this.setFooter();
        this.dtOrderChage();
        this.creatingUserSideFunctions();
        this.actionsAtEndOfBind();
    }
    //#endregion

    //#region Public Functions

    addRowCounter() {
        let m = this.model;
        if (m.grid.columns.length == 0 && m.grid["data"] != null && m.grid["data"].length > 0) {
            m.grid.columns.push(m.counterForColumns);
            $.each(m.grid["data"][0], function (k, v) {
                m.grid.columns.push({
                    title: k,
                    defaultContent: "",
                    data: k,
                    orderable: true
                });
            });
        } else {
            m.grid.columns.unshift(m.counterForColumns);
        }
    }

    setSGVGlobalVariables(self = this, m = this.model) {
        if (m.globalVariables[m.containerId] == undefined)
            m.globalVariables[m.containerId] = {};
        m.globalVariables[m.containerId][m.thisTableId] = { hasThead: 0, hasTfoot: 0, columns: {}, columnsName: {} };
    }

    setSGVArray(self = this, m = this.model) {
        if (m.gridsArray[m.containerId] !== undefined && m.gridsArray[m.containerId][m.thisTableId] !== undefined && parseInt(m.level) == 1) {
            $("#" + m.containerId).html("");
            $.each(m.gridsArray[m.containerId], function (t, v) {
                v.TableAPI.clear();
                v.TableAPI.destroy();
                v.TableObject.destroy();
            });
            m.gridsArray[m.containerId] = {};
        }
    }

    columnsTrace(self = this, m = this.model) {
        for (let j = 0; j < m.grid.columns.length; j++) {
            let TempColumn = m.grid.columns[j];
            if (TempColumn != null && TempColumn.rowGrouping !== undefined && TempColumn.rowGrouping !== null && TempColumn.rowGrouping.enable === true) {
                rowGrouping = { rowNumber: j, cssClass: TempColumn.rowGrouping.cssClass };
            }
            if (TempColumn != null /*&& TempColumn.visible == true*/) {
                let CellName = TempColumn["data"] ? TempColumn["data"].trim() : "";
                m.allTitleTh += "<th data-tbodyid='" + m.tbodyId + "'>" + TempColumn.title + "</th>";
                m.allFooterTh += "<th data-tbodyid='" + m.tbodyId + "' id='Footer_" + m.thisTableId + "_" + CellName + "'>" + TempColumn.title + "</th>";
                if (TempColumn["functions"] && TempColumn["functions"] !== null && TempColumn["functions"].length > 0) {
                    m.globalVariables[m.containerId][m.thisTableId].columnsName[CellName] = m.cellIndex;
                    $.each(TempColumn["functions"], function (k, FuncArray) {
                        if (m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex] == undefined)
                            m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex] = {};
                        if (FuncArray.funcName == "CumulativeSum") {
                            m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex]["cumulative"] = 0;
                        } else if (["OnClick", "Calc"].includes(FuncArray.funcName) && [0, 2].includes(parseInt(FuncArray.section))) {
                            m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex]["footerValue"] = 0;
                            m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex]["name"] = CellName;
                            m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex]["tfootOnClick"] = true;
                            m.globalVariables[m.containerId][m.thisTableId].columns[m.cellIndex]["footerText"] = FuncArray.footerText !== undefined ? FuncArray.footerText : null;
                        }
                        if (m.functionsList[FuncArray.funcName]["FuncListBuild"]) {
                            $.each(m.functionsList[FuncArray.funcName]["FuncListBuild"], function (k, DTMethodName) {
                                if (m.totalFunctionDetails[DTMethodName][m.cellIndex] == undefined) {
                                    m.totalFunctionDetails[DTMethodName][m.cellIndex] = { CellName: CellName, CellFuncArray: [] }
                                }
                                m.totalFunctionDetails[DTMethodName][m.cellIndex]["CellFuncArray"].push(FuncArray);
                            });
                        }
                    });
                }
                m.cellIndex++;
                m.allColumns.push(TempColumn);
                let t = { ...TempColumn };
                t.data = sapGridViewTools.toCamelCase(t.data);
                m.allCamelCaseColumns.push(t);
                m.mainColumnsName[CellName] = "";
                //-start-headerComplex------------------
                if (TempColumn.visible == false)
                    m.numberOfUnVisibleCells++;
                if (m.grid.headerComplex != null && m.grid.headerComplex.length > 0) {
                    let temp = self.headerComplex(m.grid, CellName);
                    if (cellsToBeMerged.includes(CellName)) {
                    }
                    else if (temp.title != "") {
                        cellsToBeMerged = temp.columnsToBeMerged;
                        let colspan = cellsToBeMerged.length + m.numberOfUnVisibleCells;
                        m.numberOfUnVisibleCells = 0;
                        AllComplexedTh += "<th rowspan='1' colspan='" + colspan + "'>" + temp.title + "</th> ";
                    }
                    else if (TempColumn.visible !== false) {
                        AllComplexedTh += "<th rowspan='1' colspan='1'>" + TempColumn.title + "</th> ";
                    }
                }
                //-end-headerComplex-------------------------------
            }
        }
        m.grid.columns = m.allColumns;
    }

    removeNullsFromTotalFunctionDetails(self = this, m = this.model) {
        //Remove null elements
        m.totalFunctionDetails["createdCell"] = m.totalFunctionDetails["createdCell"].filter(function (el) {
            return el != null;
        });
        m.totalFunctionDetails["orderChange"] = m.totalFunctionDetails["orderChange"].filter(function (el) {
            return el != null;
        });
    }

    addEmptyTable(self = this, m = this.model) {
        let TheadTr = "";
        TheadTr += m.allComplexedTh !== "" ? " <tr class='DT_TrThead'> " + m.allComplexedTh + " </tr> " : "";
        TheadTr += "<tr class='DT_TrThead'>" + m.allTitleTh + "</tr><tr data-flag='hide' class='DT_TrFilters'>" + m.allTitleTh + "</tr>";
        let TfootTr = "<tr class='DT_TrTfoot'>" + m.allTitleTh + "</tr><tr data-flag='hide' class='DT_TrFilters'>" + m.allFooterTh + "</tr>";
        let TableHtml = "";
        let GridContentHtml_Start = "<div class='SGV_GridContent SGV_ActiveTabContent " + m.containerId + "GridContent' id='" + m.thisTabContentID + "'>";
        TableHtml += "<div class='SGV_LoadingContainer'><i class='fa fa-circle-o-notch fa-spin fa-3x fa-fw SGV_LoadingIcon'></i></div>";
        TableHtml += "<table id='" + m.thisTableId + "' data-gridparameters='" + m.textGridParameters + "' class='Grid SGV_Grids'>";
        TableHtml += "<thead class='DT_Thead " + m.theadId + "' id='" + m.theadId + "'>" + TheadTr + "</thead>";
        TableHtml += "<tbody id='" + m.tbodyId + "' ></tbody>";
        TableHtml += "<tfoot class='DT_Tfoot' >" + TfootTr + "</tfoot>";
        TableHtml += "</table>";
        let GridContentHtml_End = "</div>";
        let ThisTab = self.tabsControl();

        if (m.level == "1" && ThisTab != "Exist") {
            TableHtml = "<div class='SGV_TabsContainer sortableSection " + m.containerId + "TabsContainer' id='" + m.tabsContainerID + "'>" + ThisTab + "</div>" + GridContentHtml_Start + TableHtml + GridContentHtml_End;
            $("#" + m.containerId).append(TableHtml);
        } else if (ThisTab == "Exist") {
            $("#" + m.thisTabContentID).html(TableHtml);
        } else {
            TableHtml = GridContentHtml_Start + TableHtml + GridContentHtml_End;
            $("#SGV_" + m.containerId + "Tabs").append(ThisTab);
            $("#" + m.containerId).append(TableHtml);
        }
        m.thisTable = $("#" + m.containerId + " table");
    }

    setDefaultOptions(self = this, m = this.model) {
        m.thisColumnDefs = self.addFunctionsToColumns();
        m.defaultOptions = self.getDefaultOptions();

        m.defaultOptions["columnDefs"] = m.thisColumnDefs;
        m.defaultOptions["columns"] = m.grid.columns;
        m.defaultOptions["data"] = m.grid.data;
    }

    removeLastSameGrid(self = this, m = this.model) {
        //m.grid = Options_SGV(m.grid, m.grid.options);
        if (m.gridsArray[m.containerId] !== undefined && m.gridsArray[m.containerId][m.thisTableId] !== undefined) {
            //اینجا نباید مجدد سطرها را اضافه کرد و جدول را رسم کرد چرا که بعد از این شرط یکبار دیتاتیبل را صدا میزنیم و تمام داده ها را به آن میدهیم
            m.gridsArray[m.containerId][m.thisTableId]["TableAPI"].clear();
            m.gridsArray[m.containerId][m.thisTableId]["TableAPI"].destroy();
            self.tabSwitch();
            $("#" + m.tabsContainerID).show();
            $("#" + m.thisTabIDme).html(m.thisTabTitle);
        }
    }

    addRowGrouping(self = this, m = this.model) {
        //Row Grouping
        if (m.rowGrouping !== null) {
            let columnsCount = m.defaultOptions.columns.length;
            m.defaultOptions["order"] = [[rowGrouping.rowNumber, 'asc']];
            m.defaultOptions["drawCallback"] = function (settings) {
                let api = this.api();
                let rows = api.rows({ page: 'current' }).nodes();
                let last = null;

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
    }

    addServerSideProcessing(self = this, m = this.model) {
        if (m.grid.serverSide == true && m.grid.processing == true) {
            let thisWebMethodName = "SapGridServerSide";
            m.defaultOptions["ajax"] = {
                url: document.location.origin + document.location.pathname + "?handler=" + thisWebMethodName,
                type: 'POST',
                data: function (d) {
                    //console.log(d);
                    d.gridInfo = {
                        containerId: m.grid.containerId,
                        serverSide: m.grid.serverSide,
                        processing: m.grid.processing,
                        gridTitle: m.grid.gridTitle,
                        gridName: m.gridName
                    }
                    d.customData = m.customData;
                    //delete d.columns;
                    // d.custom = $('#myInput').val();
                }
            };
            //m.defaultOptions["order"] = [[0, 'desc']];
            m.defaultOptions["processing"] = true;
            m.defaultOptions["serverSide"] = true;
            m.defaultOptions["columns"] = m.allCamelCaseColumns;
        }
    }

    exec(self = this, m = this.model) {
        m.tableObject = $("#" + m.thisTableId).DataTable(m.defaultOptions);
        m.thisTable.closest(".dataTables_wrapper").addClass("DT_Container");
        m.thisTableAPI = new $.fn.dataTable.Api(m.tableObject);
    }

    setTableInfo(self = this, m = this.model) {
        m.tableInfo = {
            TableId: m.thisTableId,
            TableObject: m.tableObject,
            TableAPI: m.thisTableAPI,
            SGVGlobalVariables: m.globalVariables,
            Columns: m.grid.columns,
            ContainerId: m.containerId
        };
    }

    onDraw(self = this, m = this.model) {
        let t = 0
        m.tableObject.on("draw", function () {
            //console.log("onDraw init");
            if (t > 0) {
                //console.log("onDraw t > 0");
                //ServerSideGridBind(m.newRData, m.gridName, m.grid, m.level, m.gridFirstText, m.customData);
                //if (m.grid.serverSide) {
                //    self.setDefaultOptions(self, m);
                //    console.log("is serverSide");
                //}
                self.onDrawSetFooter();
            }
            t = 1;
        });
    }
    //#endregion

    //#region functions in columns
    Separator(cellInfo, self = this, m = this.model) {
        let ThisCellNewData = cellInfo.cellData;
        if ([0, null, 'null', '0', '', ' ', undefined, NaN, 'undefined', 'NaN'].includes(ThisCellNewData) === false) {
            ThisCellNewData = sapGridViewTools.strToFloat(ThisCellNewData);
            if ([null, 0, 2].includes(parseInt(cellInfo.funcArray.section)) == false) {
                let minFraction = 0;
                let maxFraction = 0;
                if (!Number.isInteger(ThisCellNewData)) {
                    /*مینیمم نباید یک باشد، چراکه فرمت عددی برای اکسل غیرقابل محاصبه می شود، یا هیچ عددی بعد ممیز نباشد یا بیشتر از یک عدد باید باشد*/
                    minFraction = cellInfo.funcArray.minimumFractionDigits > cellInfo.funcArray.maximumFractionDigits ? cellInfo.funcArray.maximumFractionDigits : cellInfo.funcArray.minimumFractionDigits;
                    maxFraction = cellInfo.funcArray.maximumFractionDigits;
                    if (maxFraction > 1 && minFraction < 1) {
                        minFraction = 2;
                    }
                    else if (maxFraction == 1) {
                        ThisCellNewData = sapGridViewTools.strToFloat(Number(ThisCellNewData).toFixed(1));
                        minFraction = 2;
                        maxFraction = 2;
                    }
                }
                ThisCellNewData = ThisCellNewData.toLocaleString(cellInfo.funcArray.locales, {
                    minimumFractionDigits: minFraction,
                    maximumFractionDigits: maxFraction
                });
            }
        }
        return [ThisCellNewData, ThisCellNewData];
    }

    TextFeature(cellInfo, self = this, m = this.model) {
        let ThisCellNewData = cellInfo.cellData;
        let ThisTDNewData = cellInfo.td;
        if (cellInfo.funcArray.condition !== null) {
            ThisTDNewData = cellInfo.cellData;
            let condition = cellInfo.funcArray.condition;
            let isTrueCssClass = cellInfo.funcArray.isTrueCssClass;
            let isFalseCssClass = cellInfo.funcArray.isFalseCssClass;
            let isTrueText = cellInfo.funcArray.isTrueText;
            let isFalseText = cellInfo.funcArray.isFalseText;
            let strReplace = cellInfo.funcArray.strReplace;
            let numericCheckInText = cellInfo.funcArray.numericCheckInText;
            let numericCheckInCondition = cellInfo.funcArray.numericCheckInCondition;
            isTrueText = cellInfo.funcArray.isTrueText != null ? sapGridViewTools.customStrReplace(isTrueText, cellInfo.cellName, ThisCellNewData, true) : ThisCellNewData;
            isFalseText = cellInfo.funcArray.isFalseText != null ? sapGridViewTools.customStrReplace(isFalseText, cellInfo.cellName, ThisCellNewData, true) : ThisCellNewData;
            $.each(cellInfo.rowData, function (key, val) {
                let valNumber = ["", undefined, NaN, null, 'undefined', 'NaN', 'null'].includes(sapGridViewTools.strToFloat(val)) == false && sapGridViewTools.strToFloat(val) ? sapGridViewTools.strToFloat(val) : val;
                let vTest = numericCheckInText ? valNumber : val;
                let vCondition = numericCheckInCondition ? valNumber : val;


                if (cellInfo.funcArray.isTrueText !== null)
                    isTrueText = sapGridViewTools.customStrReplace(isTrueText, key, vTest, true);
                if (cellInfo.funcArray.isFalseText !== null)
                    isFalseText = sapGridViewTools.customStrReplace(isFalseText, key, vTest, true);
                condition = sapGridViewTools.customStrReplace(condition, key, vCondition, true);
            });
            if (eval(condition)) {
                $.each(strReplace, function (key, val) {
                    isTrueText = sapGridViewTools.customStrReplace(isTrueText, key, val);
                });
                $(cellInfo.td).addClass(isTrueCssClass);
                let ThisTDNewData = isTrueText;
            } else {
                $(cellInfo.td).addClass(isFalseCssClass);
                let ThisTDNewData = isFalseText;
            }
        } else {
            self.errorMessage("TextFeatureConditionNotFound");
        }
        /*
         * ThisCellNewData ممکن است با تغییر دیتای اصلی
         *مشکلی ایجاد شود مثلا  برای اعداد منفی
         * پرانتز بگذاریم یا متن را تبدیل به تگ اچ تی ام ال کنیم
         */
        if (cellInfo.funcArray.changeOriginalData == true)
            ThisCellNewData = ThisTDNewData;
        return [ThisCellNewData, ThisTDNewData];
    }

    MiladiToJalali(cellInfo, self = this, m = this.model) {
        let ThisCellNewData = cellInfo.cellData;
        if (["", " ", undefined, NaN, null, 'undefined', 'NaN', 'null', "-"].includes(ThisCellNewData) === false) {
            ThisCellNewData = ThisCellNewData.toString();
            let ThisDate = ThisCellNewData ? ThisCellNewData.trim() : "";
            ThisDate = ThisDate.replace(/-/g, "/").split('.')[0];
            ThisDate = ThisDate.replace("T", " ");
            if (sapGridViewTools.isValidDate(ThisDate)) {
                let dateTime = new Date(Date.parse(ThisDate));
                let j = jalaliConvert.gregorianToJalali(new Array(
                    dateTime.getFullYear(),
                    dateTime.getMonth() + 1,
                    dateTime.getDate()
                ));
                let hour = cellInfo.funcArray.zeroPad && cellInfo.funcArray.zeroPad == true ? ("0" + dateTime.getHours()).slice(-2) : dateTime.getHours();
                let minute = cellInfo.funcArray.zeroPad && cellInfo.funcArray.zeroPad == true ? ("0" + dateTime.getMinutes()).slice(-2) : dateTime.getMinutes();
                let second = cellInfo.funcArray.zeroPad && cellInfo.funcArray.zeroPad == true ? ("0" + dateTime.getSeconds()).slice(-2) : dateTime.getSeconds();
                switch (cellInfo.funcArray.outPut) {
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
        return [ThisCellNewData, ThisCellNewData];
    }

    Calc(cellInfo, self = this, m = this.model) {
        let ThisCellNewData = cellInfo.cellData;
        let tmpFormula = cellInfo.funcArray.formula;
        let tmpOperator = cellInfo.funcArray.operator;
        let CellIndex = m.globalVariables[m.containerId][m.thisTableId]["columnsName"][cellInfo.cellName];
        m.globalVariables[m.containerId][m.thisTableId].hasThead = parseInt(cellInfo.funcArray.section) === 0 && m.globalVariables[m.containerId][m.thisTableId].hasThead === 0 ? 1 : m.globalVariables[m.containerId][m.thisTableId].hasThead;
        m.globalVariables[m.containerId][m.thisTableId].hasTfoot = parseInt(cellInfo.funcArray.section) === 2 && m.globalVariables[m.containerId][m.thisTableId].hasTfoot === 0 ? 1 : m.globalVariables[m.containerId][m.thisTableId].hasTfoot;
        if (parseInt(cellInfo.funcArray.section) === 1) {
            $.each(cellInfo.rowData, function (key, val) {
                valNumber = val;
                if (cellInfo.funcArray.numericCheck == true)
                    valNumber = sapGridViewTools.strToFloat(val);
                tmpFormula = sapGridViewTools.customStrReplace(tmpFormula, key, valNumber, true);
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
        if ([0, 2].includes(parseInt(cellInfo.funcArray.section)) == true && tmpOperator == 0 && cellInfo.funcArray.formula == null) {//verticalSum
            m.globalVariables[m.containerId][m.thisTableId].columns[CellIndex]["name"] = cellInfo.cellName;
            m.globalVariables[m.containerId][m.thisTableId].columns[CellIndex]["footerValue"] = sapGridViewTools.strToFloat(m.globalVariables[m.containerId][m.thisTableId].columns[CellIndex]["footerValue"]);
            m.globalVariables[m.containerId][m.thisTableId].columns[CellIndex]["footerValue"] += sapGridViewTools.strToFloat(ThisCellNewData);
        } else if ([0, 2].includes(parseInt(cellInfo.funcArray.section)) == false && tmpOperator == 0 && cellInfo.funcArray.formula == null) {
            self.errorMessage("VerticalSumWithoutSelectFooterSection");
        }
        return [ThisCellNewData, ThisCellNewData];
    }

    OnClick(cellInfo, self = this, m = this.model) {
        let rowAllData = {};
        rowAllData["FuncArray"] = cellInfo.funcArray;
        rowAllData["RowData"] = cellInfo.rowData;
        rowAllData["MainColumnsName"] = m.mainColumnsName;
        let ThisRowData = JSON.stringify(rowAllData);
        ThisRowData = sapGridViewTools.base64Encode(ThisRowData);
        let cssClass = cellInfo.funcArray.cssClass ? cellInfo.funcArray.cssClass : "btn btn-link text-danger p-0 m-0";
        let webMethodName = cellInfo.funcArray.webMethodName ? cellInfo.funcArray.webMethodName : "SapGridEvent";
        let hrefLink = cellInfo.funcArray.hrefLink ? cellInfo.funcArray.hrefLink : "javascript:void(0)";
        let javaScriptMethodName = cellInfo.funcArray.javaScriptMethodName ? cellInfo.funcArray.javaScriptMethodName : null;
        let nextTabTitle = cellInfo.funcArray.nextTabTitle ? cellInfo.funcArray.nextTabTitle : "";
        let httpRequestType = cellInfo.funcArray.httpRequestType ? parseInt(cellInfo.funcArray.httpRequestType) : 0;
        let ThisCellNewData = cellInfo.cellData;
        if (parseInt(cellInfo.funcArray.section) == 1 && cellInfo.funcArray.enable == true) {
            switch (httpRequestType) {
                case 0:
                    //Ajax
                    ThisCellNewData = "<a class='" + cssClass + "' data-nexttabtitle='" + nextTabTitle + "' data-cellname='" + cellInfo.cellName + "' data-containerid='" + m.containerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + m.thisTableId + "' onclick='sapGridViewOnClick.ajaxClick(this)'>" + cellInfo.cellData + "</a>";
                    break;
                case 1:
                    //PageLink
                    ThisCellNewData = "<a class='" + cssClass + "' data-row='" + ThisRowData + "' data-tableid='" + m.thisTableId + "' href='" + hrefLink + "'>" + cellInfo.cellData + "</a>";
                    break;
                case 2:
                    //CallJavaScriptMethod
                    ThisCellNewData = "<a class='" + cssClass + "' data-javascriptmethod='" + javaScriptMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + m.thisTableId + "' onclick='sapGridViewOnClick.callJavaScriptMethodClick(this)'>" + cellInfo.cellData + "</a>";
                    break;
                default:
                    ThisCellNewData = "<a class='" + cssClass + "' data-nexttabtitle='" + nextTabTitle + "' data-cellname='" + cellInfo.cellName + "' data-containerid='" + m.containerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + m.thisTableId + "' onclick='sapGridViewOnClick.ajaxClick(this)'>" + cellInfo.cellData + "</a>";
                /*ThisCellNewData = cellInfo.cellData;
                //PostBack type
                //ThisCellNewData = '<a id="cphMain_SapGrid" href="javascript:__doPostBack(\'ctl00$cphMain$SapGrid\',\'\')">ssssssss</a>';
                ThisCellNewData = "<a class='" + cssClass + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + m.thisTableId + "'  onclick='sapGridViewOnClick.postBackClick(this)'>" + cellInfo.cellData + "</a>";
                break;*/
            }
        }
        return [ThisCellNewData, ThisCellNewData];
    }

    SAPCheckBox(cellInfo, self = this, m = this.model) {
        let rowAllData = {};
        rowAllData["FuncArray"] = cellInfo.funcArray;
        rowAllData["RowData"] = cellInfo.rowData;
        let ThisRowData = JSON.stringify(rowAllData);
        ThisRowData = sapGridViewTools.base64Encode(ThisRowData);
        let cssClass = cellInfo.funcArray.cssClass ? cellInfo.funcArray.cssClass : "btn btn-link text-danger p-0 m-0";
        let webMethodName = cellInfo.funcArray.webMethodName ? cellInfo.funcArray.webMethodName : "SapGridEvent";
        let ThisCellNewData = cellInfo.cellData;
        if (cellInfo.funcArray.enable == true) {
            ThisCellNewData = "<input type='checkbox' class='" + cssClass + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + m.thisTableId + "' onclick='SAPCheckBoxClick_SGV(this)' >" + cellInfo.cellData;
        }
        return [ThisCellNewData, ThisCellNewData];
    }

    CumulativeSum(cellInfo, self = this, m = this.model) {
        let ThisCellNewData = cellInfo.cellData;
        let CellIndex = m.globalVariables[m.containerId][m.thisTableId]["columnsName"][cellInfo.cellName];
        if (cellInfo.funcArray.sourceField && cellInfo.funcArray.sourceField !== null) {
            let tmpFormula = cellInfo.funcArray.sourceField;
            $.each(cellInfo.rowData, function (key, val) {
                let valNumber = sapGridViewTools.strToFloat(val);
                tmpFormula = sapGridViewTools.customStrReplace(tmpFormula, key, valNumber, true);
            });
            m.globalVariables[m.containerId][m.thisTableId].columns[CellIndex]["cumulative"] += eval(tmpFormula);
            ThisCellNewData = m.globalVariables[m.containerId][m.thisTableId].columns[CellIndex]["cumulative"];
        } else
            self.errorMessage("CumulativeSumSourceFieldNotFound");
        return [ThisCellNewData, ThisCellNewData];
    }
    //#endregion

    //#region refactor old functions
    addGeneralSearch(self = this, m = this.model) {
        if (m.grid.serverSide == false && m.grid.processing == false) {
            let ThisTableID = m.tableInfo.TableId;
            let x = $("#" + ThisTableID).closest(".dataTables_wrapper").find(".dt-buttons").children(".DTCustomGeneralSearch").length;
            if (x == 0 && m.grid.options["gridSearchTextBox"] === true) {
                $("#" + ThisTableID).closest(".dataTables_wrapper")
                    .find(".dt-buttons")
                    .prepend("<input type='search' class='form-control form-control-sm DTCustomGeneralSearch " + ThisTableID + "GeneralSearch' placeholder='جستجو در همه ستون ها' aria-controls='" + ThisTableID + "'>")
                    .children(".DTCustomGeneralSearch")
                    .on("keyup change", function () {
                        self.searchCustomized($(this), m.tbodyId, "GeneralSearch", m.tableInfo);
                    });
            }
            if (m.thisTable.closest(".dataTables_scroll").length) {
                let HeaderFiltersThead = m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollHead thead.DT_Thead");
                m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody thead.DT_Thead").removeClass("DT_Thead").addClass("DT_TheadWidthControl");
                m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody tr.DT_TrThead").removeClass("DT_TrThead").addClass("DT_TrWidthControl");
                m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody tfoot").remove();
                m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollBody .DT_TrFilters").remove();
                //m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollFoot tr.DT_TrTfoot").removeClass("DT_TrTfoot").addClass("DT_TrWidthControl").find("th").html("");
                m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollFoot tr.DT_TrTfoot").removeClass("DT_TrTfoot").addClass("DT_TrWidthControl");
                m.thisTable.closest(".dataTables_scroll").find(".dataTables_scrollFoot .DT_TrFilters").removeClass("DT_TrFilters").addClass("DT_TrTfootCalc");
            } else {
                let HeaderFiltersThead = m.thisTable.find("thead.DT_Thead");
            }
        }
    }

    addFilters(self = this, m = this.model) {
        if (m.grid.options["dropDownFilterButton"] === true || m.grid.options["columnsSearchButton"] === true) {
            let self = this, m = this.model;
            let i = 0;
            $.each(m.grid.columns, function (k, column) {

                let TdId = "Footer_" + m.thisTableId + "_" + column.data;
                //if (!column.rowGrouping || column.rowGrouping == null || column.rowGrouping.enable == false) {
                if (column.visible == true) {

                    let ThisTh = $("#" + m.theadId).find(".DT_TrFilters").children("th").eq(i);
                    let title = ThisTh.text();
                    let width = parseInt(ThisTh.width()) + 20;
                    let selectTag = "";
                    let inputTag = "";
                    if (m.grid.options["dropDownFilterButton"] === true && m.grid.columns[k].dropDownFilter == true) {
                        selectTag = "<span class='DT_ColumnFilterContainer'><select style='min-width:" + width + "px;' class='DT_ColumnFilter " + m.thisTableId + "ColumnFilter' data-columnnum='" + k + "' data-tbodyid='" + m.tbodyId + "'><option value=''> " + title + " </option></select></span>";
                    }
                    if (m.grid.options["columnsSearchButton"] === true) {
                        inputTag = "<span class='DT_ColumnSearchContainer'><input style='min-width:" + width + "px;' type='text' class='form-control-sm input-sm DT_ColumnSearch " + m.thisTableId + "ColumnSearch' data-columnnum='" + k + "' data-tbodyid='" + m.tbodyId + "' placeholder='" + title + "'></span>";
                    }
                    i++;
                    ThisTh.html(selectTag + inputTag);
                }
                //}
            });
        }
    }

    fillDropDownFilters(self = this, m = this.model) {
        if (m.grid.options["dropDownFilterButton"] === false)
            return;
        let sumWidth = 0;
        let i = 0;
        m.thisTableAPI.columns().every(function (k) {
            let column = this;
            if (column.visible() == true && (!column.rowGrouping || column.rowGrouping == null || column.rowGrouping.enable == false)) {
                let columnIndex = column.index();
                if (m.grid.columns[columnIndex].dropDownFilter == true) {
                    let thisFixedTh = $("#" + m.theadId).find(".DT_TrFilters th").eq(i);
                    let thisScrollTh = $(".dataTables_scrollBody thead tr").find("th").eq(i);
                    let title = thisFixedTh.text();
                    let width = title.length * 10;
                    sumWidth += width;
                    let tbodyid = thisFixedTh.attr("data-tbodyid");

                    thisScrollTh.html("");
                    let tmp = [];
                    column.data().unique().sort().each(function (d, j) {
                        d = "<span>" + d + "</span>";
                        let txt = $(d).text();
                        if (txt !== "" && $.inArray(txt, tmp) == -1) {
                            tmp.push(txt);
                            thisFixedTh.find("select").append('<option value="' + txt + '">' + txt + '</option>');
                        }
                    });
                    tmp = [];
                }
                i++;
            }
        });
    }

    onChangeFilters(self = this, m = this.model) {
        $(".DT_ColumnFilter").on('change', function () {
            let allInput = $(this).closest("tr").find("select");
            let tbodyid = $(this).attr("data-tbodyid");
            self.searchCustomized(allInput, tbodyid, "ColumnFilter", m.tableInfo);
        });
        $(".DT_ColumnSearch").on('keyup change', function () {
            let allInput = $(this).closest("tr").find("input");
            let tbodyid = $(this).attr("data-tbodyid");
            self.searchCustomized(allInput, tbodyid, "ColumnSearch", m.tableInfo);
        });
        $(".dataTables_filter input").on('keyup', function () {
            let allInput = $(this);
            let tbodyid = $(this).closest(".dataTables_wrapper").find("th").attr("data-tbodyid");
            self.searchCustomized(allInput, tbodyid, "GeneralSearch", m.tableInfo);
        });
    }

    keepScrolHeight(self = this, m = this.model) {
        m.thisTable.closest('.dataTables_scrollBody').each(function () {
            let ThisGridRows = $(this);
            ThisGridRows[0].style.setProperty('max-height', m.grid.containerHeight + 'px', 'important');
            let id = ThisGridRows.find("table.dataTable").attr("id");
            //let scrollArray = (typeof scrollArray != 'undefined' && scrollArray instanceof Array) ? scrollArray : [];
            let scrollArray = [];
            let thisGridScrollValue = 0;
            if (typeof (scrollArray[id]) !== 'undefined') {
                thisGridScrollValue = scrollArray[id];
            }
            ThisGridRows.scroll(function () {
                if (ThisGridRows.html().length) {
                    scrollArray[id] = ThisGridRows.scrollTop();
                }
            });
            ThisGridRows.scrollTop(thisGridScrollValue);
        });
    }

    setFooter(self = this, m = this.model) {
        if (m.globalVariables[m.containerId][m.thisTableId].hasThead === 1 && m.globalVariables[m.containerId][m.thisTableId].hasTfoot === 1)
            return;
        //console.log(m.globalVariables[m.containerId][m.thisTableId]);

        let ThisFooter = $("#" + m.tableInfo["TableId"]).closest(".dataTables_wrapper").find(".dataTables_scrollFoot");
        ThisFooter.find(".DT_TrTfootCalc").find("th").html("");
        let showFooter = false;
        let ColumnNumberIncludingStatus = 0;
        $.each(m.globalVariables[m.containerId][m.tableInfo["TableId"]].columns, function (cellIndex, cell) {
            if (m.tableInfo.Columns[cellIndex].visible === true) {
                let ThisValue = cell.footerValue;
                let ThisDisplayValue = "";
                let ThisVal_OpenTag = "";
                let ThisVal_CloseTag = "";
                let ItemCss = m.tableInfo.Columns[cellIndex].className;
                let cellName = cell.name;
                let TdId = "Footer_" + m.tableInfo.TableId + "_" + cell.name;
                $.each(m.tableInfo.Columns[cellIndex].functions, function (k, FuncArray) {
                    if ([2].includes(parseInt(FuncArray.section))) {

                        if (FuncArray.funcName == "OnClick") {
                            let rowAllData = {};
                            rowAllData["FuncArray"] = FuncArray;
                            rowAllData["RowData"] = {};
                            let ThisRowData = JSON.stringify(rowAllData);
                            ThisRowData = sapGridViewTools.base64Encode(ThisRowData);
                            let cssClass = FuncArray.cssClass ? FuncArray.cssClass : "btn btn-link text-danger p-0 m-0";
                            let cellText = FuncArray.footerText != null ? FuncArray.footerText : ThisValue;
                            let webMethodName = FuncArray.webMethodName ? FuncArray.webMethodName : "SapGridEvent";
                            let nextTabTitle = FuncArray.nextTabTitle ? FuncArray.nextTabTitle : "";
                            ThisValue = cellText;
                            if (FuncArray.enable == true) {
                                ThisVal_OpenTag += "<a class='" + cssClass + "' data-nexttabtitle='" + nextTabTitle + "' data-cellname='" + cellName + "' data-containerid='" + ContainerId + "' data-webmethodname='" + webMethodName + "' data-row='" + ThisRowData + "' data-tableid='" + m.tableInfo["TableId"] + "' onclick='sapGridViewOnClick.ajaxClick(this)'>"
                                ThisValue = cellText;
                                ThisVal_CloseTag += "</a>";
                            }
                        }
                        else if (FuncArray.funcName == "Calc" && FuncArray.formula != null) {
                            let tmpFormula = FuncArray.formula;
                            $.each(m.globalVariables[m.containerId][m.tableInfo["TableId"]].columns, function (key, val) {
                                if ([null, undefined, NaN, 'undefined', 'NaN', 'null'].includes(val["footerValue"]) == false && [null, undefined, NaN, 'undefined', 'NaN', 'null'].includes(val["name"]) == false) {
                                    let footerValue = val["footerValue"];
                                    let columnName = val["name"];
                                    footerValue = sapGridViewTools.customStrReplace(footerValue.toString(), ",", "");
                                    tmpFormula = sapGridViewTools.customStrReplace(tmpFormula, columnName, footerValue, true);
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
                            ThisDisplayValue = sapGridViewTools.strToFloat(ThisValue).toLocaleString(FuncArray.locales, { minimumFractionDigits: FuncArray.minimumFractionDigits, maximumFractionDigits: FuncArray.maximumFractionDigits });
                        }
                        else if (FuncArray.funcName == "TextFeature" && FuncArray.condition != null) {

                            let condition = FuncArray.condition;
                            let isTrueCssClass = FuncArray.isTrueCssClass;
                            let isFalseCssClass = FuncArray.isFalseCssClass;
                            let isTrueText = FuncArray.isTrueText;
                            let isFalseText = FuncArray.isFalseText;
                            let strReplace = FuncArray.strReplace;
                            $.each(m.globalVariables[m.containerId][m.tableInfo["TableId"]].columns, function (key, val) {
                                let footerValue = [undefined, NaN, null, 'undefined', 'NaN', 'null', 'false', false, ""].includes(val["footerValue"]) == false ? val["footerValue"] : 0;
                                let columnName = [undefined, NaN, null, 'undefined', 'NaN', 'null', 'false', false, ""].includes(val["name"]) == false ? val["name"] : false;
                                condition = sapGridViewTools.customStrReplace(condition, columnName, footerValue, true);
                                footerValue = columnName == cellName && ThisDisplayValue !== "" ? ThisDisplayValue : footerValue;
                                isTrueText = sapGridViewTools.customStrReplace(isTrueText, columnName, footerValue, true);
                                isFalseText = sapGridViewTools.customStrReplace(isFalseText, columnName, footerValue, true);
                            });
                            if (eval(condition)) {
                                $.each(strReplace, function (key, val) {
                                    isTrueText = sapGridViewTools.customStrReplace(isTrueText, key, val);
                                });
                                ThisDisplayValue = [null, undefined, NaN, "", "null"].includes(isTrueText) === false ? isTrueText : ThisValue;
                                ItemCss = [null, undefined, NaN, 'undefined', 'NaN', 'null', ""].includes(isTrueCssClass) === false ? isTrueCssClass : ItemCss;
                            } else {
                                ThisDisplayValue = [null, undefined, NaN, "", "null"].includes(isFalseText) === false ? isFalseText : ThisValue;
                                ItemCss = [null, undefined, NaN, 'undefined', 'NaN', 'null', ""].includes(isFalseCssClass) === false ? isFalseCssClass : ItemCss;
                            }
                        }
                        m.globalVariables[m.containerId][m.tableInfo["TableId"]].columns[cellIndex]["footerValue"] = ThisValue;
                    }
                });
                let TempThisVal = ThisDisplayValue !== "" ? ThisVal_OpenTag + ThisDisplayValue + ThisVal_CloseTag : ThisVal_OpenTag + ThisValue + ThisVal_CloseTag;
                if (["undefined", undefined, "NaN", NaN].includes(TempThisVal) === false) {
                    $("#" + TdId).html(TempThisVal);
                    ThisFooter.find(".DT_TrWidthControl").children("th." + cell.name + "_Class").html(TempThisVal);
                    //ThisFooter.find(".DT_TrTfootCalc th").eq(cellIndex).html(TempThisVal); not Work after a column to be visible false
                }
                if (ItemCss != "") {
                    $("#" + TdId).addClass(ItemCss);
                    //ThisFooter.find(".DT_TrTfootCalc th").eq(cellIndex).addClass(ItemCss); not Work after a column to be visible false
                }
                showFooter = true;
                ColumnNumberIncludingStatus++;
            }
        });
        if (showFooter)
            ThisFooter.show();
    }

    dtOrderChage(self = this, m = this.model) {
        m.tableObject.on("draw", function (changeEvent) {
            if (m.grid.counterColumn == true)
                self.counterColumn(m.tableInfo.TableObject);
            if (m.totalFunctionDetails["orderChange"].length > 0) {
                $.each(m.totalFunctionDetails["orderChange"], function (c, DTOrderChange) {
                    let CellIndex = m.globalVariables[m.containerId][m.tableInfo.TableId]["columnsName"][DTOrderChange.CellName];
                    if (DTOrderChange) {
                        $.each(DTOrderChange["CellFuncArray"], function (kf, CellFunc) {
                            if (CellFunc) {
                                let fn = window["SGV_" + CellFunc.funcName + "_AfterDraw"]; //SGV_CumulativeSum_AfterDraw
                                if (typeof fn === "function") {
                                    fn.apply(window, [CellIndex, CellFunc, m.totalFunctionDetails["orderChange"], m.thisColumnDefs, m.tableInfo]);
                                }
                            }
                        });
                    }
                });
            }
        }).draw();
    }

    creatingUserSideFunctions(self = this, m = this.model) {
        if (typeof DataTableCallBackData === "function") {
            DataTableCallBackData({
                TableId: m.thisTableId,
                TableObject: m.tableObject,
                TableAPI: m.thisTableAPI,
                AllData: m.newRData,
                SGVGlobalVariables: m.globalVariables,
                ContainerId: m.containerId
            });
        }
    }

    actionsAtEndOfBind(self = this, m = this.model) {
        if (m.rowGrouping !== null)
            m.tableObject.columns([m.rowGrouping.rowNumber]).visible(false, false).draw();

        m.gridsArray[m.containerId] = m.gridsArray[m.containerId] == undefined ? {} : m.gridsArray[m.containerId];
        m.gridsArray[m.containerId][m.thisTableId] = { TableId: m.thisTableId, TableObject: m.tableObject, TableAPI: m.thisTableAPI };
    }

    headerComplex(grid, CellName) {
        let self = this, m = this.model;
        let res = { title: "", columnsToBeMerged: [] };
        $.each(grid.headerComplex, function (k, headerComplexData) {
            let columnsToBeMerged = headerComplexData.columnsToBeMerged;
            let title = headerComplexData.title;
            if (columnsToBeMerged.includes(CellName)) {
                res = { title: title, columnsToBeMerged: columnsToBeMerged };
                return false;
            }
        });
        return res;
    }

    tabsControl(self = this, m = this.model) {
        let ThisTab = "";
        if ($("#" + m.thisTabID).length > 0) {
            ThisTab = "Exist";
        } else {
            ThisTab = "<div class='SGV_Tab SGV_ActiveTabTitle " + m.containerId + "Tab' ";
            ThisTab += "id='" + m.thisTabID + "' ";
            ThisTab += "onclick='sapGridViewOnClick.tabOnClick(this)' ";
            ThisTab += "data-containerid='" + m.containerId + "' ";
            ThisTab += "data-tabid='" + m.thisTabID + "' ";
            ThisTab += "data-contentid='" + m.thisTabContentID + "' > ";
            ThisTab += m.thisTabTitle;
            ThisTab += " </div> ";
            //ThisTab += "<i class='fa fa-remove SGV_CloseTab' onclick='sapGridViewOnClick.closeTab(this);' data-tabid='" + m.thisTabID + "' data-contentid='" + m.thisTabContentID + "'></i>";
        }
        if (parseInt(m.level) > 1) {
            self.tabSwitch();
            $("#" + m.tabsContainerID).show();
        }
        return ThisTab;
    }

    tabSwitch(self = this, m = this.model) {
        $("." + m.containerId + "Tab").removeClass("SGV_ActiveTabTitle");
        $("." + m.containerId + "GridContent").removeClass("SGV_ActiveTabContent");
        $("#" + m.thisTabID).addClass("SGV_ActiveTabTitle").css("display", "");
        $("#" + m.thisTabContentID).addClass("SGV_ActiveTabContent").css("display", "");
    }

    addFunctionsToColumns(self = this, m = this.model) {
        if (m.totalFunctionDetails["createdCell"].length > 0) {
            let UniqueFunctionCallCheckArray = []; //اگر این آرایه نباشد هر تابع محاسبه دوبار خوانده میشود
            $.each(m.totalFunctionDetails["createdCell"], function (k, v) {
                let cellIndex = m.globalVariables[m.containerId][m.thisTableId]["columnsName"][v.CellName];
                if (v) {
                    m.thisColumnDefs.push({
                        targets: cellIndex,
                        createdCell: function (td, cellData, rowData, RowIndex, c) {
                            let ThisCellNewData = cellData;
                            $.each(v["CellFuncArray"], function (kf, FuncArray) {
                                let fnName = FuncArray.funcName //+ "_ServerCall_SGV";
                                //let UniqueFunctionCallCheck = m.containerId + m.thisTableId + v.CellName + tmp + RowIndex + cellIndex + "-" + kf;
                                let UniqueFunctionCallCheck = m.containerId + m.thisTableId + v.CellName + RowIndex + cellIndex + "-" + kf;
                                UniqueFunctionCallCheckArray = UniqueFunctionCallCheckArray == null ? [] : UniqueFunctionCallCheckArray;;
                                if (UniqueFunctionCallCheckArray.includes(UniqueFunctionCallCheck) == false) {
                                    UniqueFunctionCallCheckArray.push(UniqueFunctionCallCheck);

                                    let tbodyCheck = ["TextFeature", "Separator"].includes(FuncArray.funcName);
                                    let section = parseInt(FuncArray.section);

                                    if (typeof self[fnName] === "function") {

                                        if ((tbodyCheck === true && section == 1) || tbodyCheck === false) {
                                            let cellInfo = {
                                                td: td,
                                                cellData: ThisCellNewData,
                                                rowData: rowData,
                                                funcArray: FuncArray,
                                                cellName: v.CellName
                                            };
                                            let tmp = self[fnName](cellInfo);
                                            ThisCellNewData = tmp[0];
                                            let ThisTDNewData = tmp[1];

                                            $(td).html(ThisTDNewData);
                                        }

                                    }
                                    rowData[v.CellName] = $("<span>" + ThisCellNewData + "</span>").text();
                                }
                            });
                        }
                    });
                }
            });
            UniqueFunctionCallCheckArray = null;
        }
        return m.thisColumnDefs;
    }

    getDefaultOptions(self = this, m = this.model) {
        let customOptions = m.grid.options;
        let ExportTitle = "";
        if (customOptions["titleRowInExelExport"] === true) {
            ExportTitle = [null, undefined, NaN, ""].includes(m.grid.gridTitle) !== true ? sapGridViewTools.customStrReplace(m.grid.gridTitle, " ", "-") + "-" : "";
            ExportTitle += jalaliConvert.jalaliToday() + "-" + (new Date()).getHours() + ":" + (new Date()).getMinutes() + ":" + (new Date()).getSeconds();
        }
        let DefaultOptions = {
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
            //    let pagination = $(this).closest('.dataTables_wrapper').find('.dataTables_paginate');
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

        let CopyButton = {
            extend: 'copy',
            titleAttr: "کپی کل جدول",
            title: "",
            text: '<i class="fa fa-copy DataTableIcons"></i>',
            footer: true,
            exportOptions: {
                columns: ':visible'
            }
        };
        let PrintButton = {
            extend: 'print',
            titleAttr: "پرینت",
            text: '<i class="fa fa-print DataTableIcons"></i>',
            footer: true,
            exportOptions: {
                columns: ':visible'
            }
        };
        let ExcelButton = {
            extend: 'excel',
            text: '<i class="fa fa-file-excel-o DataTableIcons"></i>',
            titleAttr: "خروجی اکسل",
            title: ExportTitle,
            footer: true,
            exportOptions: {
                columns: ':visible'
            }
        };
        let ColumnsSearchButton = {
            text: '<i class="fa fa-search DataTableIcons DatatableHeaderFilters"></i>',
            titleAttr: "جستجو در هر ستون",
            action: function (e, dt, node, conf) {
                sapGridViewTools.datatableHeaderFilters($(this)[0]["node"]["attributes"]["aria-controls"]["value"], "Search");
            }
        };
        let DropDownFilterButton = {
            text: '<i class="fa fa-filter DataTableIcons DatatableHeaderFilters"></i>',
            titleAttr: "فیلتر روی هر ستون",
            action: function (e, dt, node, conf) {
                sapGridViewTools.datatableHeaderFilters($(this)[0]["node"]["attributes"]["aria-controls"]["value"], "Filter");
            }
        };
        let RecycleButton = {
            text: '<i class="fa fa-recycle DataTableIcons"></i>',
            titleAttr: "حذف وضعیت های ذخیره شده",
            action: function (e, dt, node, conf) {
                dt.state.clear();
            }
        };
        let RemoveAllFilters = {
            text: '<i class="fa fa-remove DataTableSmallIcons"></i><i class="fa fa-search DataTableIcons"></i>',
            titleAttr: "حذف همه فیلترها و جستجوها",
            action: function (e, dt, node, conf) {
                self.clearAllFilters(dt, m.thisTableId);
            }
        };


        //-Start---customizeButtons-------
        if (m.grid.customizeButtons) {
            for (const [k, btnArray] of Object.entries(m.grid.customizeButtons)) {
                if (btnArray.javascriptMethodName == null || btnArray.javascriptMethodName.trim() == "") {
                    self.errorMessage("CustomizeButtonJsUnDefine", btnArray.javascriptMethodName);
                } else if (btnArray.buttonName == null || btnArray.buttonName == 0) {
                    self.errorMessage("CustomizeButtonNameUnDefine", btnArray.buttonName);
                } else {
                    customButton(btnArray);
                }
            }
        }
        function customButton(btnArray) {
            let fn = window[btnArray.javascriptMethodName];
            if (typeof fn === "function") {
                let btnOptions = fn.apply(window, [{ YourData: btnArray.data }]);

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
                self.errorMessage("CustomizeButtonJsFunNotFound", btnArray.javascriptMethodName);
            }
        }
        //-End---customizeButtons-------

        if (customOptions["copyButton"] === true) DefaultOptions.buttons.push(CopyButton);
        if (customOptions["printButton"] === true) DefaultOptions.buttons.push(PrintButton);
        if (customOptions["excelButton"] === true) DefaultOptions.buttons.push(ExcelButton);
        if (customOptions["columnsSearchButton"] === true) DefaultOptions.buttons.push(ColumnsSearchButton);
        if (customOptions["dropDownFilterButton"] === true) DefaultOptions.buttons.push(DropDownFilterButton);
        if (customOptions["recycleButton"] === true) DefaultOptions.buttons.push(RecycleButton);

        DefaultOptions.buttons.push(RemoveAllFilters);

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

    onDrawSetFooter(self = this, m = this.model) {
        let ThisFooter = $("#" + m.tableInfo["TableId"]).closest(".dataTables_wrapper").find(".dataTables_scrollFoot");
        ThisFooter.find(".DT_TrTfootCalc").find("th").html("");
        let showFooter = false;
        let ContainerId = m.tableInfo.ContainerId;
        let ColumnNumberIncludingStatus = 0;
        $.each(m.globalVariables[ContainerId][m.tableInfo["TableId"]].columns, function (CellIndex, cell) {
            if (m.tableInfo.Columns[CellIndex].visible === true) {
                let ThisValue = cell.footerValue;
                let ThisDisplayValue = "";
                let ThisVal_OpenTag = "";
                let ThisVal_CloseTag = "";
                let ItemCss = m.tableInfo.Columns[CellIndex].className;
                let cellName = cell.name;
                let tdTitle = "";
                $.each(m.tableInfo.Columns[CellIndex].functions, function (k, FuncArray) {
                    if ([2].includes(parseInt(FuncArray.section))) {
                        if (FuncArray.funcName == "Calc" && FuncArray.formula == null && FuncArray.operator == 0) {

                            // Total over all pages
                            let total = m.tableInfo.TableAPI
                                .column(CellIndex)
                                .data()
                                .reduce(function (a, b) {
                                    return sapGridViewTools.strToFloat(a) + sapGridViewTools.strToFloat(b);
                                }, 0);

                            // Total over this page
                            let pageTotal = m.tableInfo.TableAPI
                                .column(CellIndex, { page: 'current' })
                                .data()
                                .reduce(function (a, b) {
                                    return sapGridViewTools.strToFloat(a) + sapGridViewTools.strToFloat(b);
                                }, 0);

                            // Total over filter
                            let pageFilter = m.tableInfo.TableAPI
                                .column(CellIndex, { filter: 'applied' })
                                .data()
                                .reduce(function (a, b) {
                                    return sapGridViewTools.strToFloat(a) + sapGridViewTools.strToFloat(b);
                                }, 0);


                            tdTitle += "جمع این صفحه: ";
                            tdTitle += pageTotal.toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                            tdTitle += "\nجمع فیلتر شده ها: ";
                            tdTitle += pageFilter.toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                            tdTitle += "\nجمع کل: ";
                            tdTitle += total.toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 3 });
                            ThisValue = pageFilter;
                            //$(m.tableInfo.TableAPI.column(CellIndex).footer()).html('$' + pageTotal + ' ( $' + total + ' total)');
                        }
                        else if (FuncArray.funcName == "Separator" && ThisValue !== undefined) {
                            ThisDisplayValue = sapGridViewTools.strToFloat(ThisValue).toLocaleString(FuncArray.locales, { minimumFractionDigits: FuncArray.minimumFractionDigits, maximumFractionDigits: FuncArray.maximumFractionDigits });
                        }
                        else if (FuncArray.funcName == "TextFeature" && FuncArray.condition != null) {

                            let condition = FuncArray.condition;
                            let isTrueCssClass = FuncArray.isTrueCssClass;
                            let isFalseCssClass = FuncArray.isFalseCssClass;
                            let isTrueText = FuncArray.isTrueText;
                            let isFalseText = FuncArray.isFalseText;
                            let strReplace = FuncArray.strReplace;
                            $.each(m.globalVariables[ContainerId][m.tableInfo["TableId"]].columns, function (key, val) {
                                let footerValue = [undefined, NaN, null, 'undefined', 'NaN', 'null', 'false', false, ""].includes(val["footerValue"]) == false ? val["footerValue"] : 0;
                                let columnName = [undefined, NaN, null, 'undefined', 'NaN', 'null', 'false', false, ""].includes(val["name"]) == false ? val["name"] : false;
                                condition = sapGridViewTools.customStrReplace(condition, columnName, footerValue, true);
                                footerValue = columnName == cellName && ThisDisplayValue !== "" ? ThisDisplayValue : footerValue;
                                isTrueText = sapGridViewTools.customStrReplace(isTrueText, columnName, footerValue, true);
                                isFalseText = sapGridViewTools.customStrReplace(isFalseText, columnName, footerValue, true);
                            });
                            if (eval(condition)) {
                                $.each(strReplace, function (key, val) {
                                    isTrueText = sapGridViewTools.customStrReplace(isTrueText, key, val);
                                });
                                ThisDisplayValue = [null, undefined, NaN, "", "null"].includes(isTrueText) === false ? isTrueText : ThisValue;
                                ItemCss = [null, undefined, NaN, 'undefined', 'NaN', 'null', ""].includes(isTrueCssClass) === false ? isTrueCssClass : ItemCss;
                            } else {
                                ThisDisplayValue = [null, undefined, NaN, "", "null"].includes(isFalseText) === false ? isFalseText : ThisValue;
                                ItemCss = [null, undefined, NaN, 'undefined', 'NaN', 'null', ""].includes(isFalseCssClass) === false ? isFalseCssClass : ItemCss;
                            }
                        }
                        m.globalVariables[ContainerId][m.tableInfo["TableId"]].columns[CellIndex]["footerValue"] = ThisValue;
                    }
                });
                let TempThisVal = ThisDisplayValue !== "" ? ThisVal_OpenTag + ThisDisplayValue + ThisVal_CloseTag : ThisVal_OpenTag + ThisValue + ThisVal_CloseTag;
                let TdId = "Footer_" + m.tableInfo.TableId + "_" + cell.name; //dataTables_scrollFoot
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

    clearAllFilters(TableObject, ThisTableID) {
        $("." + ThisTableID + "GeneralSearch").val("");
        $("." + ThisTableID + "ColumnSearch").val("");
        $("." + ThisTableID + "ColumnFilter").prop("selectedIndex", 0);
        TableObject.search('').columns().search('').draw();
    }

    searchCustomized(allInput, tbodyid, SearchType, TableInfo) {
        let self = this, m = this.model;
        let emptyArray = [null, "", NaN, undefined];
        let dataSearch = {};
        let i = 0;
        let TableObject = TableInfo.TableObject;
        let ThisTableID = TableInfo.TableID;
        TableObject.search('').columns().search('').draw(); //Clear All Search & Filters
        let elementsType = "";
        allInput.each(function () {
            let inputData = $(this).val();
            //let inputData = sapGridViewTools.arabicToPersianChar(inputData); نباید تبدیل به فارسی شود، چراکه جایی
            let columnNum = $(this).attr("data-columnnum");
            let elementType = $(this).prop("nodeName");
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
                let persianText = sapGridViewTools.arabicToPersianChar(v.inputData);
                let arabicText = sapGridViewTools.persianToArabicChar(v.inputData);
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

    counterColumn(TableObject) {
        let self = this, m = this.model;
        let i = 1;
        TableObject.rows({ order: 'applied' }).every(function (rowIdx, tableLoop, rowLoop) {
            TableObject.cell(rowIdx, 0).data(i++);
        });
    }

    static errorMessage(errorKey, otherInfo = null) {
        let errorArray = {
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
    //#endregion
}

class sapGridViewTools {
    static strToFloat(str) {
        str = str !== undefined && str != null && str != NaN && $("<span>" + str + "</span>").text().trim() != "" ? $("<span>" + str + "</span>").text().trim() : 0;
        str = this.customStrReplace(str.toString(), "/", ".");
        str = str.replace(/[^\d.-]/g, '');
        return parseFloat(str);
    }

    static customStrReplace(str, searchValue, replaceValue, matchWholeWord, findAllAndReplace) {
        findAllAndReplace = findAllAndReplace ? findAllAndReplace : true;
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

    static isValidDate(dateObject) {
        return new Date(dateObject).toString() !== 'Invalid Date';
    }

    static datatableHeaderFilters(TableId, type) {
        let ThisDataTable = $("#" + TableId).closest(".dataTables_wrapper");
        let ThisThead = ThisDataTable.find(".DT_TrFilters");
        let AllTheadFiltersID = ThisDataTable.find(".DT_TrFilters").find(".DT_ColumnFilterContainer, .DT_ColumnSearchContainer");
        let ThisTheadFilterID = ThisDataTable.find(".DT_TrFilters").find(".DT_Column" + type + "Container");
        let flag = ThisThead.attr("data-flag");
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

    static base64Decode(str) {
        // Unicode support
        return decodeURIComponent(atob(str).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    }

    static base64Encode(str) {
        // Unicode support
        return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
            function toSolidBytes(match, p1) {
                return String.fromCharCode('0x' + p1);
            }));
    }

    static arabicToPersianChar(str) {
        //let c = str.replace(/ى/g,"ی");
        let c = str.replace(/ي/g, "ی");
        c = c.replace(/ئ/g, "ی");
        c = c.replace(/ة/g, "ه");
        c = c.replace(/ك/g, "ک");
        c = c.replace(/ؤ/g, "و");
        return c;
    }

    static persianToArabicChar(str) {
        //let c = str.replace(/ى/g,"ي");
        //let c = str.replace(/ى/g,"ی");
        let c = str.replace(/ی/g, "ي");
        c = c.replace(/ک/g, "ك");
        return c;
    }

    static toCamelCase(str) {
        if (typeof str !== 'string' || str.length === 0) {
            return str;
        }
        return str.charAt(0).toLowerCase() + str.slice(1);
    }

    static isKeyExist(array, k) {
        if (array[k] != undefined && array[k] != NaN && k != null) {
            return true;
        }
        return false;
    }

    static copyAndCamelCaseIgnore(source, destination) {
        Object.keys(destination).forEach(key => {
            const lowerKey = key.toLowerCase();
            if (source.hasOwnProperty(lowerKey)) {
                destination[key] = source[lowerKey];
            } else if (source.hasOwnProperty(key)) {
                destination[key] = source[key];
            }
        });
        return destination;
    }
}

class sapGridViewOnClick {
    //#region ClientSide Methods
    static postBackClick(obj) {
        //__doPostBack('ctl00$cphMain$am', '');
        //__doPostBack('ctl00$cphMain$SapGrid', '');
        __doPostBack('SapGrid', '');
        //javascript: __doPostBack('ctl00$cphMain$SapGridPostBack', '');
    }

    static callJavaScriptMethodClick(obj) {
        let CallBackData = sapGridViewTools.base64Decode(obj.dataset.row);
        CallBackData = JSON.parse(CallBackData);
        let ThisTableID = obj.dataset.tableid;
        let JavaScriptMethodName = obj.dataset.javascriptmethod;
        let gridParameters = $("#" + ThisTableID).attr("data-gridparameters");
        gridParameters = sapGridViewTools.base64Decode(gridParameters);
        CallBackData["GridParameters"] = JSON.parse(gridParameters);
        let fn = window[JavaScriptMethodName];
        if (typeof fn === "function") {
            fn.apply(window, [{ TableID: ThisTableID, CallBackData: CallBackData, ClickedObject: obj }]);
        }
    }

    static ajaxClick(obj) {
        let objText = obj.text ? obj.text : "untitled";
        $(".SGV_LoadingContainer").show();
        let ThisTableID = obj.dataset.tableid;
        let ContainerId = obj.dataset.containerid;
        let cellName = obj.dataset.cellname;
        let ThisWebMethodName = obj.dataset.webmethodname;
        let GridFirstText = [null, undefined, NaN, ""].includes(obj.dataset.nexttabtitle) === false ? obj.dataset.nexttabtitle : GridFirstText;
        GridFirstText = sapGridViewTools.customStrReplace(GridFirstText, "{clickedItem}", objText, false, true);
        let cData = JSON.parse(sapGridViewTools.base64Decode(obj.dataset.row));
        let rowData = {};
        $.each(cData.RowData, function (key, val) {
            GridFirstText = sapGridViewTools.customStrReplace(GridFirstText, key, val);
            rowData[key] = val.toString();
        });
        let gridParameters = sapGridViewTools.base64Decode($("#" + ThisTableID).attr("data-gridparameters"));
        let callBackData = {
            GridParameters: JSON.parse(gridParameters),
            TableDetails: { ContainerId: ContainerId, TableID: ThisTableID, CellName: cellName },
            RowData: sapGridViewTools.copyAndCamelCaseIgnore(rowData, cData.MainColumnsName),
            FuncArray: cData.FuncArray
        };
        $.ajax({
            type: "POST",
            url: document.location.origin + document.location.pathname + "?handler=" + ThisWebMethodName,
            data: JSON.stringify(callBackData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                let d = JSON.parse(data);
                if (ThisWebMethodName == "SapGridEvent") {
                    new sapGridView(d, cData.FuncArray.Level, GridFirstText);
                }
                else {
                    let fn = window[ThisWebMethodName];
                    if (typeof fn === "function") {
                        fn.apply(window, [d, obj, SGVArray[ContainerId][ThisTableID]]);
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

    static changeData(methodName, callBackData) {
        $(".SGV_LoadingContainer").show();
        $.ajax({
            type: "POST",
            url: document.location.origin + document.location.pathname + "?handler=" + methodName,
            data: JSON.stringify(callBackData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var d = JSON.parse(data);
                new sapGridView(d, 1, "1");
                $(".SGV_LoadingContainer").hide();
            },
            error: function (error) {
                console.log(error);
                $(".SGV_LoadingContainer").hide();
                alert("خطایی در ارتباط با سرور وجود دارد");
            }
        });
    }

    static closeTab(obj) {
        let ThisTabID = obj.dataset.tabid;
        let ThisTabContentID = obj.dataset.contentid;
        $("#" + ThisTabID).hide(500);
        $("#" + ThisTabContentID).hide(500);
        //$("#" + ThisTabContentID).hide(500);

        /*$("#" + ThisTabID).closest(".SGV_TabsContainer").find(".SGV_Tab").eq(0).addClass("SGV_ActiveTabTitle");
        $("#" + ThisTabContentID).parent().find(".SGV_GridContent").eq(0).addClass("SGV_ActiveTabContent");*/
    }

    static tabOnClick(obj) {
        let ThisTabID = obj.dataset.tabid;
        let ThisTabContentID = obj.dataset.contentid;
        let ContainerId = obj.dataset.containerid;
        sapGridViewOnClick.tabSwitch(ThisTabID, ThisTabContentID, ContainerId);
    }

    static tabSwitch(ThisTabID, ThisTabContentID, ContainerId) {
        $("." + ContainerId + "Tab").removeClass("SGV_ActiveTabTitle");
        $("." + ContainerId + "GridContent").removeClass("SGV_ActiveTabContent");
        $("#" + ThisTabID).addClass("SGV_ActiveTabTitle").css("display", "");
        $("#" + ThisTabContentID).addClass("SGV_ActiveTabContent").css("display", "");
    }
    //#endregion

}

class sapGridViewModel {
    newRData = null;
    gridName = null;
    grid = null;
    level = null;
    gridFirstText = null;
    customData = null;
    //----------------------------------
    counterForColumns = null;
    functionsList = null;
    containerId = null;
    textGridParameters = null;
    extraPostfix = "";
    joinContainerAndGridName = null;
    thisTabID = null;
    thisTabTitle = null;
    thisTabContentID = null;
    tabsContainerID = null;
    allTitleTh = "";
    allFooterTh = "";
    allComplexedTh = "";
    thisColumnDefs = [];
    thisTableId = null;
    tbodyId = null;
    theadId = null;
    totalFunctionDetails = {};
    cellIndex = 0;
    mainColumnsName = {};
    allColumns = [];
    allCamelCaseColumns = [];
    rowGrouping = null;
    cellsToBeMerged = [];
    numberOfUnVisibleCells = 0;
    thisTable = null;
    thisTableAPI = null;
    defaultOptions = {};
    tableObject = null;
    tableInfo = {};
    globalVariables = {};
    gridsArray = {};

    setModelProps(newRData, gridName, dataArray, level, gridFirstText) {
        let m = this;
        m.newRData = newRData;
        m.gridName = gridName;
        m.grid = dataArray;
        m.level = level;
        m.gridFirstText = gridFirstText;
        m.customData = newRData.CustomData;

        m.containerId = m.grid && m.grid.containerId ? m.grid.containerId : null;
        m.textGridParameters = m.grid && m.grid.gridParameters ? sapGridViewTools.base64Encode(JSON.stringify(m.grid.gridParameters)) : null;
        m.joinContainerAndGridName = m.containerId + "_" + m.gridName;
        m.thisTabID = m.joinContainerAndGridName + "_ThisTab" + "_Level" + m.level + m.extraPostfix;
        m.thisTabTitle = m.level + "- " + m.gridFirstText;
        m.thisTabContentID = m.joinContainerAndGridName + "_ThisTabContent" + "_Level" + m.level + m.extraPostfix;
        m.tabsContainerID = "SGV_" + m.containerId + "Tabs";
        m.thisTableId = m.joinContainerAndGridName + "_Level" + m.level + m.extraPostfix;
        m.tbodyId = m.joinContainerAndGridName + "_Level" + m.level + "GridTbody" + "_Level" + m.level + m.extraPostfix;
        m.theadId = m.thisTableId + "Thead";
        m.counterForColumns = {
            title: "#",
            defaultContent: "",
            data: "SGVRadifCounter",
            orderable: false,
            visible: m.grid && m.grid.counterColumn ? m.grid.counterColumn : true,
            dropDownFilter: true,
            rowGrouping: false
        };
        m.functionsList = {
            CumulativeSum: { FuncListBuild: ["createdCell", "orderChange"] },
            Calc: { FuncListBuild: ["createdCell"] },
            MiladiToJalali: { FuncListBuild: ["createdCell"] },
            Separator: { FuncListBuild: ["createdCell"] },
            SAPCheckBox: { FuncListBuild: ["createdCell"] },
            OnClick: { FuncListBuild: ["createdCell"] },
            TextFeature: { FuncListBuild: ["createdCell"] }
        };
        m.totalFunctionDetails = {
            createdCell: [],
            orderChange: []
        };
        return m;
    }
}

class jalaliConvert {
    /* jalali.js  Gregorian to Jalali and inverse date convertor
 * Copyright (C) 2001  Roozbeh Pournader <roozbeh@sharif.edu>
 * Copyright (C) 2001  Mohammad Toossi <mohammad@bamdad.org>
 * Copyright (C) 2003,2008  Behdad Esfahbod <js@behdad.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You can receive a copy of GNU Lesser General Public License at the
 * World Wide Web address <http://www.gnu.org/licenses/lgpl.html>.
 *
 * For licensing issues, contact The FarsiWeb Project Group,
 * Computing Center, Sharif University of Technology,
 * PO Box 11365-8515, Tehran, Iran, or contact us the
 * email address <FWPG@sharif.edu>.
 */

    /* Changes:
     * 
     * 2008-Jul-32:
     *  Use a remainder() function to fix conversion of ancient dates
     *	(before 1600 gregorian).  Reported by Shamim Rezaei.
     *
     * 2003-Mar-29:
     *		Ported to javascript by Behdad Esfahbod
     *
     * 2001-Sep-21:
     *	Fixed a bug with "30 Esfand" dates, reported by Mahmoud Ghandi
     *
     * 2001-Sep-20:
     *	First LGPL release, with both sides of conversions
     */

    static gregorianDaysInMonth = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
    static jalaliDaysInMonth = new Array(31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29);

    static div(a, b) {
        return Math.floor(a / b);
    }

    static remainder(a, b) {
        return a - this.div(a, b) * b;
    }

    static gregorianToJalali(g /* array containing year, month, day*/) {
        let gy, gm, gd;
        let jy, jm, jd;
        let g_day_no, j_day_no;
        let j_np;

        let i;

        gy = g[0] - 1600;
        gm = g[1] - 1;
        gd = g[2] - 1;

        g_day_no = 365 * gy + this.div((gy + 3), 4) - this.div((gy + 99), 100) + this.div((gy + 399), 400);

        for (i = 0; i < gm; ++i)
            g_day_no += this.gregorianDaysInMonth[i];

        if (gm > 1 && ((gy % 4 == 0 && gy % 100 != 0) || (gy % 400 == 0)))
            /* leap and after Feb */
            ++g_day_no;
        g_day_no += gd;

        j_day_no = g_day_no - 79;

        j_np = this.div(j_day_no, 12053);
        j_day_no = this.remainder(j_day_no, 12053);

        jy = 979 + 33 * j_np + 4 * this.div(j_day_no, 1461);
        j_day_no = this.remainder(j_day_no, 1461);


        if (j_day_no >= 366) {
            jy += this.div((j_day_no - 1), 365);
            j_day_no = this.remainder((j_day_no - 1), 365);
        }
        for (i = 0; i < 11 && j_day_no >= this.jalaliDaysInMonth[i]; ++i) {
            j_day_no -= this.jalaliDaysInMonth[i];
        }

        jm = i + 1;
        jd = j_day_no + 1;
        if (jy.toString().length == 1) jy = "0" + jy;
        if (jm.toString().length == 1) jm = "0" + jm;
        if (jd.toString().length == 1) jd = "0" + jd;
        return new Array(jy, jm, jd);
    }

    static jalaliToGregorian(j /* array containing year, month, day*/) {
        j = j.split("/");
        let gy, gm, gd;
        let jy, jm, jd;
        let g_day_no, j_day_no;
        let leap;

        let i;

        jy = j[0] - 979;
        jm = j[1] - 1;
        jd = j[2] - 1;

        j_day_no = 365 * jy + this.div(jy, 33) * 8 + this.div((this.remainder(jy, 33) + 3), 4);
        for (i = 0; i < jm; ++i)
            j_day_no += this.jalaliDaysInMonth[i];

        j_day_no += jd;

        g_day_no = j_day_no + 79;

        gy = 1600 + 400 * this.div(g_day_no, 146097); /* 146097 = 365*400 + 400/4 - 400/100 + 400/400 */
        g_day_no = this.remainder(g_day_no, 146097);

        leap = 1;
        if (g_day_no >= 36525) /* 36525 = 365*100 + 100/4 */ {
            g_day_no--;
            gy += 100 * this.div(g_day_no, 36524); /* 36524 = 365*100 + 100/4 - 100/100 */
            g_day_no = this.remainder(g_day_no, 36524);

            if (g_day_no >= 365)
                g_day_no++;
            else
                leap = 0;
        }

        gy += 4 * this.div(g_day_no, 1461); /* 1461 = 365*4 + 4/4 */
        g_day_no = this.remainder(g_day_no, 1461);

        if (g_day_no >= 366) {
            leap = 0;

            g_day_no--;
            gy += this.div(g_day_no, 365);
            g_day_no = this.remainder(g_day_no, 365);
        }

        for (i = 0; g_day_no >= this.gregorianDaysInMonth[i] + (i == 1 && leap); i++)
            g_day_no -= this.gregorianDaysInMonth[i] + (i == 1 && leap);
        gm = i + 1;
        gd = g_day_no + 1;
        if (gy.toString().length == 1) gy = "0" + gy;
        if (gm.toString().length == 1) gm = "0" + gm;
        if (gd.toString().length == 1) gd = "0" + gd;
        return new Array(gy, gm, gd);
    }

    static sqlDateToJalali(dateTime) {
        dateTime = dateTime.trim();
        let res = dateTime.split(" ");
        let date = res[0];
        let time = res[1];
        let dateArray = date.split("-");
        j = this.gregorianToJalali(new Array(
            dateArray[0],
            dateArray[1],
            dateArray[2]
        ));
        return j[0] + "/" + j[1] + "/" + j[2];
    }

    static jalaliToday() {
        let today = new Date();
        let j = this.gregorianToJalali(new Array(
            today.getFullYear(),
            today.getMonth() + 1,
            today.getDate()
        ));
        return j[0] + "/" + j[1] + "/" + j[2];
    }
}