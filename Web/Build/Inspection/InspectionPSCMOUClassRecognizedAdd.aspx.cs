using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;



public partial class Inspection_InspectionPSCMOUClassRecognizedAdd : PhoenixBasePage
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
        //ddlExternalOrganization.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
        //ddlExternalOrganization.DataTextField = "FLDORGANIZATIONNAME";
        //ddlExternalOrganization.DataValueField = "FLDORGANIZATIONID";
        //ddlExternalOrganization.DataBind();
        //ddlExternalOrganization.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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
                //Label lit2 = new Label();
                //lit.Height = 25;
                //lit2.Text = "</br></br></br>";
                //Pnlcolumnlabel.Controls.Add(lit2);
                //Label lit3 = new Label();
                //lit.Height = 25;
                //lit3.Text = "</br></br></br>";
                //Pnlcolumnlabel.Controls.Add(lit3);                
                RadComboBox flagperf = new RadComboBox();
                flagperf.RenderMode = RenderMode.Lightweight;
                flagperf.Filter = RadComboBoxFilter.Contains;
                flagperf.MarkFirstMatch = true;
                flagperf.Width = 150;
                flagperf.ID = ddlflagperf + i.ToString();
                flagperf.EnableLoadOnDemand = true;
                flagperf.AppendDataBoundItems = true;
                //RadComboBox combo = (RadComboBox)sender;
                flagperf.Items.Add(new RadComboBoxItem("Yes", "1"));
                flagperf.Items.Add(new RadComboBoxItem("No", "0"));
                flagperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                Pnlcolumnlabel.Controls.Add(flagperf);
                Label lit1 = new Label();
                lit1.Text = "</br></br>";
                Pnlcolumnlabel.Controls.Add(lit1);

                if (ViewState["FLDCLASSID"] != null && ViewState["ACTION"].ToString() == "edit")
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixInspectionPSCMOUMatrix.PSCMOUShipRecognizedOrganisationEdit(General.GetNullableInteger(ViewState["FLDCLASSID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        //RadComboBox ddlflagper = FindControl(ddlflagperf + (i).ToString()) as RadComboBox;                       
                        ucClassName.SelectedValue = dt.Rows[i]["FLDCLASSID"].ToString();
                        ucClassName.Text = dt.Rows[i]["FLDCLASSNAME"].ToString();
                        ucClassName.Enabled = false;
                        flagperf.SelectedValue = dt.Rows[i]["FLDISRECOGNIZEDYN"].ToString();
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
                scriptClosePopup += "<script language='javaScript' id='City'>" + "\n";
                scriptClosePopup += "fnReloadList('City');";
                scriptClosePopup += "</script>" + "\n";

                string scriptRefreshDontClose = "";
                scriptRefreshDontClose += "<script language='javaScript' id='CityNew'>" + "\n";
                scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
                scriptRefreshDontClose += "</script>" + "\n";

                if (ucClassName.SelectedValue == "")
                {
                    ucError.ErrorMessage = "Class Performance is required";
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
                        //string column = "";
                        string columnvalue = "";
                        for (int i = 0; i < n; i++)
                        {
                            RadTextBox MyTextBox = new RadTextBox();
                            Label mouid = new Label();
                            if (MyTextBox != null)
                            {
                                //string columnheader = "";
                                string ss = "";
                                string mou = "";
                                string ddl = "ddlflagperf";
                                RadComboBox ddlflagperf = FindControl(ddl + (i).ToString()) as RadComboBox;

                                ss = Request.Form["ddlflagperf" + (i).ToString()];
                                //ss = (Request.Form["RadComboBox1"].ToString());
                                mou = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();

                                //columnheader = "COLUMN" + i.ToString();
                                //DataTable ds = new DataTable();
                                //ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                                //column = ds.Rows[0][columnheader].ToString();
                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUShipRecognizedOrganisationInsert(
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
                    ucStatus.Text = "Information Added";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "CityNew", scriptClosePopup);
                }
                if (ViewState["FLDCLASSID"] != null && ViewState["ACTION"].ToString() == "edit")
                {
                    int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
                    if (n > 0)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            string mou = "";
                            string ddl = "ddlflagperf";
                            RadComboBox ddlflagperf = FindControl(ddl + (i).ToString()) as RadComboBox;
                            mou = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();
                            PhoenixInspectionPSCMOUMatrix.PSCMOUShipRecognizedOrganisationUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                 General.GetNullableInteger(ucClassName.SelectedValue),
                                                                 General.GetNullableInteger(ddlflagperf.SelectedValue),
                                                                 General.GetNullableGuid(mou)
                                                                 //ref colorscoreid
                                                                 );



                        }

                    }
                    ucStatus.Text = "Information Updated";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "CityNew", scriptClosePopup);
                }

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}