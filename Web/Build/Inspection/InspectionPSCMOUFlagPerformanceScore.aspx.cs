using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Inspection_InspectionPSCMOUFlagPerformanceScore : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["NOOFCOLUMNS"] = null;
            ViewState["FLDFLAGPERFORMANCEID"] = null;
            DataSet ds = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows.Count;

            if (Request.QueryString["flagperformance"] != null)
                ViewState["FLDFLAGPERFORMANCEID"] = Request.QueryString["flagperformance"].ToString();
            else
                ViewState["FLDFLAGPERFORMANCEID"] = null;

            if (Request.QueryString["action"] != null)
                ViewState["ACTION"] = Request.QueryString["action"].ToString();
            else
                ViewState["ACTION"] = "";
            FlagPerformance();
        }

        //DefaultColumnEdit();
        CreateDynamicTextBox();
    }

    protected void FlagPerformance()
    {
        ddlflagperf.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUFlagPerformance();
        ddlflagperf.DataTextField = "FLDNAME";
        ddlflagperf.DataValueField = "FLDFLAGPERFORMANCEID";
        ddlflagperf.DataBind();
        ddlflagperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void CreateDynamicTextBox()
    {
        if (ViewState["NOOFCOLUMNS"].ToString() != "")
        {
            string columnheader = "";
            string txtbox = "txtbox";
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
                RadTextBox MyTextBox = new RadTextBox();
                //Assigning the textbox ID name  
                MyTextBox.ID = txtbox + i.ToString();
                MyTextBox.Width = 100;
                //MyTextBox.Height = 25;
                MyTextBox.Font.Name = "Tahoma";
                MyTextBox.Font.Size = 8;
                MyTextBox.TextMode = InputMode.SingleLine;
                //this.Controls.Add(MyTextBox);
                Pnlcolumnlabel.Controls.Add(MyTextBox);
                Label lit1 = new Label();
                //lit.Height = 25;
                lit1.Text = "</br></br>";
                Pnlcolumnlabel.Controls.Add(lit1);

                if (ViewState["FLDFLAGPERFORMANCEID"] != null && ViewState["ACTION"].ToString() == "edit")
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixInspectionPSCMOUMatrix.PSCMOUFlagColorScoreEdit(General.GetNullableInteger(ViewState["FLDFLAGPERFORMANCEID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        MyTextBox.Text = dt.Rows[i]["FLDPSCMOUWEIGHTAGE"].ToString();                        
                        ddlflagperf.SelectedValue = dt.Rows[i]["FLDFLAGPERFORMANCE"].ToString();
                        ddlflagperf.Enabled = false;
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

                if (ddlflagperf.SelectedValue == "")
                {                     
                    ucError.ErrorMessage = "Flag Performance is required";
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
                                ss = (Request.Form["txtbox" + (i).ToString()].ToString()).Replace("'", "''");
                                mou = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();

                                //columnheader = "COLUMN" + i.ToString();
                                //DataTable ds = new DataTable();
                                //ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                                //column = ds.Rows[0][columnheader].ToString();
                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUShipFlagColorScoreInsert(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                         General.GetNullableInteger(ddlflagperf.SelectedValue),
                                                                         General.GetNullableGuid(mou),
                                                                         General.GetNullableInteger(ss)
                                                                         //ref colorscoreid
                                                                         );

                                }

                            }
                        }
                    }
                }
                if (ViewState["FLDFLAGPERFORMANCEID"] != null && ViewState["ACTION"].ToString() == "edit")
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
                                ss = (Request.Form["txtbox" + (i).ToString()].ToString()).Replace("'", "''");
                                mou = ds.Tables[0].Rows[i]["FLDCOMPANYID"].ToString();
                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUFlagColorScoreUpdate(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                         General.GetNullableInteger(ddlflagperf.SelectedValue),
                                                                         General.GetNullableGuid(mou),
                                                                         General.GetNullableInteger(ss)
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


