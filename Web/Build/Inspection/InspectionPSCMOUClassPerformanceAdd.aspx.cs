using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;

public partial class Inspection_InspectionPSCMOUClassPerformanceAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        ucClassName.AddressType = ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString();

        if (!IsPostBack)
        {
            ViewState["NOOFCOLUMNS"] = null;
            ViewState["FLDCLASSID"] = null;
            DataSet ds = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows.Count;

            if (Request.QueryString["classid"] != null)
                ViewState["FLDCLASSID"] = Request.QueryString["classid"].ToString();
            else
                ViewState["FLDCLASSID"] = null;

            if (Request.QueryString["action"] != null)
                ViewState["ACTION"] = Request.QueryString["action"].ToString();
            else
                ViewState["ACTION"] = "";

            BindExternalOrganization();
        }

        //DefaultColumnEdit();
        CreateDynamicTextBox();
    }

    protected void BindExternalOrganization()
    {
    //    ddlExternalOrganization.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
    //    ddlExternalOrganization.DataTextField = "FLDORGANIZATIONNAME";
    //    ddlExternalOrganization.DataValueField = "FLDORGANIZATIONID";
    //    ddlExternalOrganization.DataBind();
    //    ddlExternalOrganization.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void CreateDynamicTextBox()
    {
        if (ViewState["NOOFCOLUMNS"].ToString() != "")
        {
            string columnheader = "";
            string ddlflagperf = "ddlflagperf";
            string lbl = "lbl";
            int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
            DataSet ds = new DataSet();
            ds = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            Pnlcolumnlabel.Controls.Clear();
            for (int i = 0; i < n; i++)
            {
                columnheader = "FLDCOMPANYNAME";
                Label lit = new Label();
                Label litid = new Label();
                lit.Width = 100;
                lit.Text = ds.Tables[0].Rows[i][columnheader].ToString();
                lit.Attributes.CssStyle.Add("margin-right", "100px");
                litid.ID = lbl + i.ToString();
                litid.Text = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();
                litid.Visible = false;
                Pnlcolumnlabel.Controls.Add(lit);
                Pnlcolumnlabel.Controls.Add(litid);
               
                RadComboBox flagperf = new RadComboBox();
                flagperf.RenderMode = RenderMode.Lightweight;
                flagperf.Filter = RadComboBoxFilter.Contains;
                flagperf.MarkFirstMatch = true;
                flagperf.Width = 150;
                flagperf.ID = ddlflagperf + i.ToString();
                flagperf.EnableLoadOnDemand = true;
                flagperf.AppendDataBoundItems = true;
                //RadComboBox combo = (RadComboBox)sender;
                flagperf.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUClassPerformance();
                flagperf.DataTextField = "FLDNAME";
                flagperf.DataValueField = "FLDCLASSSOCIETYPERFORMANCEID";
                flagperf.DataBind();
                flagperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy")); 
                Pnlcolumnlabel.Controls.Add(flagperf);

                Label lit1 = new Label();
                ////lit.Height = 25;
                lit1.Text = "</br></br>";
                Pnlcolumnlabel.Controls.Add(lit1);

                if (ViewState["FLDCLASSID"] != null && ViewState["ACTION"].ToString() == "edit")
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixInspectionPSCMOUMatrix.PSCMOUShipClassPerformanceEdit(General.GetNullableInteger(ViewState["FLDCLASSID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        //RadComboBox ddlflagper = FindControl(ddlflagperf + (i).ToString()) as RadComboBox;                       
                        ucClassName.SelectedValue = dt.Rows[i]["FLDCLASSID"].ToString();
                        ucClassName.Text = dt.Rows[i]["FLDCLASSNAME"].ToString();
                        ucClassName.Enabled = false;
                        flagperf.SelectedValue = dt.Rows[i]["FLDCLASSPERFORMANCELEVEL"].ToString();
                    }
                }

            }

        }
    }

    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string scriptClosePopup = "";
                scriptClosePopup += "<script language='javaScript' id='AddressAddNew'>" + "\n";
                scriptClosePopup += "fnReloadList('AddAddress');";
                scriptClosePopup += "</script>" + "\n";

                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='AddressAddNew'>" + "\n";
                scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
                scriptRefreshDontClose += "</script>" + "\n";

                if (ucClassName.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Society is required";
                    ucError.Visible = true;
                    return;
                }

                DataSet ds = new DataSet();
                ds = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                if (ViewState["ACTION"].ToString() == "add")
                {
                    int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
                    if (n > 0)
                    {
                        string columnvalue = "";
                        for (int i = 0; i < n; i++)
                        {
                            RadTextBox MyTextBox = new RadTextBox();
                            Label mouid = new Label();
                            if (MyTextBox != null)
                            {
                                string ss = "";
                                string mou = "";
                                string ddl = "ddlflagperf";
                                RadComboBox ddlflagperf = FindControl(ddl + (i).ToString()) as RadComboBox;

                                ss = Request.Form["ddlflagperf" + (i).ToString()];
                                mou = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();

                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUShipClassPerformanceInsert(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                         General.GetNullableInteger(ucClassName.SelectedValue),
                                                                         General.GetNullableInteger(ddlflagperf.SelectedValue),
                                                                         General.GetNullableGuid(mou)
                                                                         //ref colorscoreid
                                                                         );

                                }

                            }
                        }
                    }
                }
                if (ViewState["FLDCLASSID"] != null && ViewState["ACTION"].ToString() == "edit")
                {
                    int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
                    if (n > 0)
                    {
                        // string column = "";
                        string columnvalue = "";
                        for (int i = 0; i < n; i++)
                        {
                            RadTextBox MyTextBox = new RadTextBox();
                            if (MyTextBox != null)
                            {
                                string ss = "";
                                string mou = "";
                                string ddl = "ddlflagperf";
                                RadComboBox ddlflagperf = FindControl(ddl + (i).ToString()) as RadComboBox;

                                ss = Request.Form["ddlflagperf" + (i).ToString()];
                                mou = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();
                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUShipClassPerformanceUpdate(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                         General.GetNullableInteger(ucClassName.SelectedValue),
                                                                         General.GetNullableInteger(ddlflagperf.SelectedValue),
                                                                         General.GetNullableGuid(mou)
                                                                         //ref colorscoreid
                                                                         );
                                }

                            }
                        }

                    }
                }
                Page.ClientScript.RegisterStartupScript(typeof(Page), "AddressAddNew", scriptRefreshDontClose);
                //DefaultColumnEdit();

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}