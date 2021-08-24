<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormFBFormCreate.aspx.cs"
    Inherits="StandardFormFBFormCreate" ValidateRequest="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/content/bootstrap.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/content/formio.full.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/content/jquery.json-viewer.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/scripts/formio.full.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/scripts/jquery-3.4.1.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/scripts/jquery.json-viewer.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/scripts/lz-string-1.4.4.js"></script>
        <%--<script type="text/javascript">
            function PaneResized() {
                var sender = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                sender.set_height(browserHeight - 40);
                $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
            }
        </script>--%>
    </telerik:RadCodeBlock>
    <style>
        input[type=radio]
        {
            height: 15px;
            width: 15px;
        }
        input[type=radio] + span
        {
            font-size: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadMgr1" runat="server" EnableScriptCombine="false">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
    </telerik:RadSkinManager>
    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" />
    <eluc:TabStrip ID="menuFormbuilder" runat="server" OnTabStripCommand="menuFormbuilder_TabStripCommand">
    </eluc:TabStrip>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-8">
                <h3 class="text-center text-muted">
                    Form Type
                    <select class="form-control input" id="frmType" style="display: inline-block; width: 150px;"
                        runat="server">
                        <option value="form">Form</option>
                        <option value="wizard">Wizard</option>
                        <option value="pdf">PDF</option>
                    </select></h3>
                <div id="builder">
                </div>
            </div>
            <div class="col-sm-4">
                <h3 class="text-center text-muted">
                    JSON Schema</h3>
                <div class="card card-body bg-light jsonviewer">
                    <pre id="jsonrenderer"></pre>
                    <asp:HiddenField ID="renderJson" runat="server" />
                    <asp:HiddenField ID="signature" runat="server" />
                </div>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-sm-8 offset-sm-2">
                <h3 class="text-center text-muted">
                    Preview</h3>
                <div id="formio" class="card card-body bg-light formio-form">
                </div>
            </div>
            <div class="clearfix">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
    <script type="text/javascript">

        //    
        //       var SitePath = "<%=Session["sitepath"]%>" + "/PhoenixWebFunctions.aspx";

        var formElement = document.getElementById('formio');
       
        var builder = new Formio.FormBuilder(document.getElementById("builder"), <%=ViewState["string"].ToString()%>, {
            builder: {
                custom: {
                    title: 'User Fields',
                    weight: 10,
                    components: {
                        User: {
                            title: 'User',
                            key: 'User',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'User',
                                type: 'textfield',
                                key: 'User',
                                disabled: true,
                                input: true
                            }
                        },
                        Print: {
                            title: 'Print',
                            key: 'print',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'print',
                                type: 'button',
                                key: 'print',
                                action: 'event',
                                event: 'Print',
                                input: true
                            }
                        },
                        
                   
                        saveDraft: {
                            title: 'Save as Draft',
                            key: 'saveDraft',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Save as Draft',
                                type: 'button',
                                action: 'saveState',
                                state: 'draft',
                                key: 'saveDraft'
                            }
                        },
                         Signature: {
                            title: 'Signature',
                            icon: 'fa fa-terminal',
                            // key:'Verify',                            
                            schema: {
                                type: 'columns',
                                phoenixtype: 'signature',
                                columns: [
                                    {
                                    components: [{
                                            label: 'Sign',
                                            type: 'button',
                                            key: 'verify',
                                            phoenixtype: 'crewlist',
                                            action: 'event',
                                            event: 'Verify',
                                            tableView: true,
                                            input: true
                                        }],
                                        width: 4,
                                        offset: 0,
                                        push: 0,
                                        pull: 0

                                    },
                                     {
                                        components: [{
                                            label: 'Signature',
                                            showWordCount: false,
                                            showCharCount: false,
                                            disabled: true,
                                            tableView: true,
                                            alwaysEnabled: false,
                                            type: 'textfield',
                                            key: 'signature',
                                           // key:'username',
                                            input: true,
                                            defaultValue: '',
                                            validate: {
                                                customMessage: '',
                                                json: ''
                                            },
                                            conditional: {
                                                show: '',
                                                when: '',
                                                json: ''
                                            },
                                            widget: {
                                                type: ''
                                            },
                                            reorder: false,
                                            inputFormat: 'plain',
                                            encrypted: false,
                                            properties: {},
                                            customConditional: '',
                                            logic: []
                                        }

                                        ],
                                        width: 4,
                                        offset: 0,
                                        push: 0,
                                        pull: 0
                                    }]
                            }
                        },

//                        Signature: {
//                            title: 'Signature',
//                            icon: 'fa fa-terminal',
//                             key:'Verify',                            
//                            schema: {
//                                type: 'columns',
//                                phoenixtype: 'signature',
//                                columns: [
//                                    {
//                                        components: [{
//                                            label: 'Username',
//                                            allowMultipleMasks: false,
//                                            showWordCount: false,
//                                            showCharCount: false,
//                                            tableView: true,
//                                            alwaysEnabled: false,
//                                            type: 'select',
//                                            key: 'username',
//                                            phoenixtype: 'crewlist',
//                                            valueProperty: 'value',
//                                            template: '<span>{{ item.Crew_Name }}</span>',
//                                            input: true,
//                                            defaultValue: '',
//                                            validate: {
//                                                select: false,
//                                                customMessage: '',
//                                                json: ''
//                                            },
//                                            conditional: {
//                                                show: '',
//                                                when: '',
//                                                json: ''
//                                            },
//                                            data: {
//                                                values: [
//                                                    {
//                                                        label: '',
//                                                        value: ''
//                                                    }]
//                                            },
//                                            reorder: false,
//                                            lazyLoad: false,
//                                            selectValues: '',
//                                            disableLimit: false,
//                                            sort: '',
//                                            reference: false,
//                                            selectThreshold: 0.3,
//                                            encrypted: false,
//                                            properties: {},
//                                            customConditional: '',
//                                            logic: []
//                                        }

//                                        ],
//                                        width: 6,
//                                        offset: 0,
//                                        push: 0,
//                                        pull: 0
//                                    },
//                                    {
//                                        components: [{
//                                            label: 'Password',
//                                            showWordCount: false,
//                                            showCharCount: false,
//                                            tableView: true,
//                                            alwaysEnabled: false,
//                                            type: 'password',
//                                            key: 'password',
//                                            input: true,
//                                            defaultValue: '',
//                                            validate: {
//                                                customMessage: '',
//                                                json: ''
//                                            },
//                                            conditional: {
//                                                show: '',
//                                                when: '',
//                                                json: ''
//                                            },
//                                            properties: {},
//                                            reorder: false,
//                                            inputFormat: 'plain',
//                                            encrypted: false,
//                                            customConditional: '',
//                                            logic: []
//                                        }
//                                        ],
//                                        width: 4,
//                                        offset: 0,
//                                        push: 0,
//                                        pull: 0
//                                    },
//                                    {
//                                        components: [{
//                                            label: 'Verify',
//                                            type: 'button',
//                                            key: 'verify',
//                                            action: 'event',
//                                            event: 'Verify',
//                                            tableView: true,
//                                            input: true
//                                        }],
//                                        width: 4,
//                                        offset: 0,
//                                        push: 0,
//                                        pull: 0

//                                    },
//                                    {
//                                        components: [{
//                                            label: 'Signature',
//                                            showWordCount: false,
//                                            showCharCount: false,
//                                            disabled: true,
//                                            tableView: true,
//                                            alwaysEnabled: false,
//                                            type: 'textfield',
//                                            key: 'signature',
//                                            input: true,
//                                            defaultValue: '',
//                                            validate: {
//                                                customMessage: '',
//                                                json: ''
//                                            },
//                                            conditional: {
//                                                show: '',
//                                                when: '',
//                                                json: ''
//                                            },
//                                            widget: {
//                                                type: ''
//                                            },
//                                            reorder: false,
//                                            inputFormat: 'plain',
//                                            encrypted: false,
//                                            properties: {},
//                                            customConditional: '',
//                                            logic: []
//                                        }

//                                        ],
//                                        width: 4,
//                                        offset: 0,
//                                        push: 0,
//                                        pull: 0
//                                    }]
//                            }
                     //   },


                        Time_period: {
                            title: 'Time Period',
                            key: 'number',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'number',
                                type: 'number',
                                key: 'number',
                                input: true
                            }
                        },
                        Start_Time: {
                            title: 'Start Time',
                            key: 'starttime',
                            icon: 'fa fa-terminal',

                            schema: {
                                label: 'Start Time',
                                type: 'datetime',
                                key: 'starttime',
                                defaultDate: 'moment()',
                                format: 'dd/MMM/yyyy hh:mm a',
                                datepicker: {
                                    mindate: 'moment()',
                                    input: true
                                }
                            }

                        },
                        End_Time: {
                            title: 'End Time',
                            key: 'endtime',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'End Time',
                                type: 'datetime',
                                key: 'endtime',
                                phoenixtype: 'formexpiry',
                                refreshOn: 'data',
                                calculateValue: 'value = moment(data.startTime).add(data.number2,"hours");',
                                format: 'dd/MMM/yyyy hh:mm a',
                                // format: 'MM-dd-yyyy HH:mm',
                                defaultDate: 'moment()',
                                input: true

                            }
                        },

//                           PendingApproval_Dutyofficer: {
//                            title: 'Pending Approval-DutyOfficer',
//                            key: 'pendingapproval_dutyofficer',
//                            icon: 'fa fa-terminal',
//                            schema: {
//                                label: 'Send to Duty Officer for Approval',
//                                type: 'button',
//                                key: 'pendingapproval_dutyofficer',
//                                action: 'event',
//                                event: 'PendingApproval-dutyofficer',
//                                input: true
//                            }
//                        },
//                          PendingApproval_chiefEngineer: {
//                            title: 'Pending Approval-ChiefEngineer',
//                            key: 'pendingapproval_chiefEngineer',
//                            icon: 'fa fa-terminal',
//                            schema: {
//                                label: 'Send to Chief Engineer for Approval',
//                                type: 'button',
//                                key: 'pendingapproval_chiefEngineer',
//                                action: 'event',
//                                event: 'PendingApproval-Chiefengineer',
//                                input: true
//                            }
//                        },
//                         PendingApproval_Master: {
//                            title: 'Pending Approval-Master',
//                            key: 'pendingapproval_Master',
//                            icon: 'fa fa-terminal',
//                            schema: {
//                                label: 'Send to Master for Approval',
//                                type: 'button',
//                                key: 'pendingapproval_Master',
//                                action: 'event',
//                                event: 'PendingApproval-Master',
//                                input: true
//                            }
//                        },

                        CrewList: {
                            title: 'Crew List',
                            key: 'crewlist',
                            icon: 'fa fa-terminal',
                            schema: {
                                type: 'select',
                                label: 'Crew List',
                                key: 'crewlist',
                                phoenixtype: 'crewlist',
                                placeholder: 'Select your Crew',
                                valueProperty: 'value',
                                template: '<span>{{ item.Crew_Name }}</span>',
                                dataSrc: "values"
                            }
                        }
                    }
                },

                  custom2: {
                    title: 'Approvals',
                    weight: 10,
                    components: {
                       PendingApproval_Dutyofficer: {
                            title: 'Pending Approval-DutyOfficer',
                            key: 'pendingapproval_dutyofficer',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Send to Duty Officer for Approval',
                                type: 'button',
                                key: 'pendingapproval_dutyofficer',
                                action: 'event',
                                event: 'PendingApproval-dutyofficer',
                                input: true
                            }
                        },
                        PendingApproval_DutyEngineer: {
                            title: 'Pending Approval-DutyEngineer',
                            key: 'pendingapproval_dutyengineer',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Send to Duty Engineer for Approval',
                                type: 'button',
                                key: 'pendingapproval_dutyengineer',
                                action: 'event',
                                event: 'PendingApproval-dutyengineer',
                                input: true
                            }
                        },

                          PendingApproval_chiefOfficer: {
                            title: 'Pending Approval-ChiefOfficer',
                            key: 'pendingapproval_chiefOfficer',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Send to Chief Officer for Approval',
                                type: 'button',
                                key: 'pendingapproval_chiefOfficer',
                                action: 'event',
                                event: 'PendingApproval-ChiefOfficer',
                                input: true
                            }
                        },

                         PendingApproval_chiefEngineer: {
                            title: 'Pending Approval-ChiefEngineer',
                            key: 'pendingapproval_chiefEngineer',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Send to Chief Engineer for Approval',
                                type: 'button',
                                key: 'pendingapproval_chiefEngineer',
                                action: 'event',
                                event: 'PendingApproval-Chiefengineer',
                                input: true
                            }
                        },
                           PendingApproval_Master: {
                            title: 'Pending Approval-Master',
                            key: 'pendingapproval_Master',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Send to Master for Approval',
                                type: 'button',
                                key: 'pendingapproval_Master',
                                action: 'event',
                                event: 'PendingApproval-Master',
                                input: true
                            }
                        }
                       }
                     },

                custom1: {
                    title: 'Vessel Particulars',
                    weight: 10,
                    components: {
                        Flag: {
                            title: 'Flag',
                            key: 'FLAG',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Flag',
                                type: 'textfield',
                                key: 'FLAG',
                                disabled: true,
                                input: true
                            }
                        },
                        Type: {
                            title: 'Type',
                            key: 'TYPE',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Type',
                                type: 'textfield',
                                key: 'TYPE',
                                disabled: true,
                                input: true
                            }
                        },
                        CallSign: {
                            title: 'Call Sign',
                            key: 'CALLSIGN',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Call Sign',
                                type: 'textfield',
                                key: 'CALLSIGN',
                                disabled: true,
                                input: true
                            }
                        },
                        Owner: {
                            title: 'Owner',
                            key: 'OWNER',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Owner',
                                type: 'textfield',
                                key: 'OWNER',
                                disabled: true,
                                input: true
                            }
                        },
                        Delivery: {
                            title: 'Delivery',
                            key: 'DELIVERY',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Delivery',
                                type: 'textfield',
                                key: 'DELIVERY',
                                disabled: true,
                                input: true
                            }
                        },
                        PortOfRegisry: {
                            title: 'Portofregistry',
                            key: 'PORTOFREGISTRY',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'PortOfRegistry',
                                type: 'textfield',
                                key: 'PORTOFREGISTRY',
                                disabled: true,
                                input: true
                            }
                        },
                         GrossTonnage: {
                            title: 'Gross Tonnage',
                            key: 'GROSSTONNAGE',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Gross Tonnage',
                                type: 'textfield',
                                key: 'GROSSTONNAGE',
                                disabled: true,
                                input: true
                            }
                        },

                        Classification: {
                            title: 'Classification',
                            key: 'CLASS',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'Classification',
                                type: 'textfield',
                                key: 'CLASS',
                                disabled: true,
                                input: true
                            }
                        },
                        IMO_Number: {
                            title: 'IMO Number',
                            key: 'IMO',
                            icon: 'fa fa-terminal',
                            schema: {
                                label: 'IMO Number',
                                type: 'textfield',
                                key: 'IMO',
                                disabled: true,
                                input: true
                            }
                        }
                    }
                }

            },
            editForm: {
                textfield: [
                    {
                        key: 'api',
                        ignore: false
                    }
                ]
            }
        })

        var onForm = function (form) {            
            form.on('change', function () {                
                //subJSON.innerHTML = '';
                //subJSON.appendChild(document.createTextNode(JSON.stringify(form.submission, null, 4)));
            });
        };

        var onBuild = function (build) {                                        
            setTimeout(function () { updateJson(build) }, 200);
        };        
        var onReady = function () {    
        builder.instance.on('change', onBuild);     
//            builder.instance.on('saveComponent', onBuild);
//            builder.instance.on('editComponent', onBuild);
//            builder.instance.on('removeComponent', onBuild);  
            updateJson();            
        };

        var setDisplay = function (display) {
            builder.setDisplay(display).then(onReady);
        };

        // Handle the form selection.
        var formSelect = document.getElementById('frmType');
        formSelect.addEventListener("change", function () {
            setDisplay(this.value);
        });

        builder.instance.ready.then(onReady);
        function updateJson(build) {            
            formElement.innerHTML = '';
            renderJson.value = LZString144.compressToUTF16(JSON.stringify(builder.instance.schema, null, 4));
            Formio.createForm(formElement, builder.instance.form, { readOnly: false }).then(onForm);
            var signature = FormioUtils.searchComponents(builder.instance.schema.components, { 'phoenixtype': 'signature' });
            var sign = '';
            for (var i = 0; i < signature.length; i++) {
                var username = FormioUtils.searchComponents(signature[i].columns, { 'phoenixtype': 'crewlist' });
                if (username.length > 0)
                   // sign += '#' + username[0].key + '~,' + (username[0].properties != null && username[0].properties.rank != null ? username[0].properties.rank : 'All') + ',#`';
                   sign += '#' + username[0].key + '~,' + (username[0].properties != null && username[0].properties.rank != null ? username[0].properties.rank : 'All') + ',#`';
            }
            document.getElementById('signature').value = sign;
            $('#jsonrenderer').jsonViewer(builder.instance.schema, { collapsed: true });
        }
    </script>
</telerik:RadCodeBlock>
