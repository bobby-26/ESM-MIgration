using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Inspection_InspectionPSCMOUFlagPerformanceColorAdd : PhoenixBasePage
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
            ViewState["FLDFLAGID"] = null;
            DataSet ds = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows.Count;

            if (Request.QueryString["flagid"] != null)
                ViewState["FLDFLAGID"] = Request.QueryString["flagid"].ToString();
            else
                ViewState["FLDFLAGID"] = null;

            if (Request.QueryString["action"] != null)
                ViewState["ACTION"] = Request.QueryString["action"].ToString();
            else
                ViewState["ACTION"] = "";            
        }

        //DefaultColumnEdit();
        CreateDynamicTextBox();
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
                flagperf.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUFlagPerformance();
                flagperf.DataTextField = "FLDNAME";
                flagperf.DataValueField = "FLDFLAGPERFORMANCEID";
                flagperf.DataBind();
                flagperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                //flagperf.ItemsRequested += new RadComboBoxItemsRequestedEventHandler(flagperf_ItemsRequested);
                Pnlcolumnlabel.Controls.Add(flagperf);

                //Assigning the textbox ID name  
                //MyTextBox.ID = txtbox + i.ToString();
                //MyTextBox.Width = 100;
                ////MyTextBox.Height = 25;
                //MyTextBox.Font.Name = "Tahoma";
                //MyTextBox.Font.Size = 8;
                //MyTextBox.TextMode = InputMode.SingleLine;
                ////this.Controls.Add(MyTextBox);
                //Pnlcolumnlabel.Controls.Add(MyTextBox);
                Label lit1 = new Label();
                ////lit.Height = 25;
                lit1.Text = "</br></br>";
                Pnlcolumnlabel.Controls.Add(lit1);

                if (ViewState["FLDFLAGID"] != null && ViewState["ACTION"].ToString() == "edit")
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixInspectionPSCMOUMatrix.PSCMOUShipFlagPerformanceColorEdit(General.GetNullableInteger(ViewState["FLDFLAGID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        //RadComboBox ddlflagper = FindControl(ddlflagperf + (i).ToString()) as RadComboBox;                       
                        ucFlag.SelectedFlag = dt.Rows[i]["FLDFLAGID"].ToString();
                        flagperf.SelectedValue = dt.Rows[i]["FLDFLAGPERFORMANCE"].ToString();
                        ucFlag.Enabled = false;
                    }
                }

            }

        }
    }

    protected void flagperf_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox combo = (RadComboBox)sender;
        combo.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUFlagPerformance();
        combo.DataTextField = "FLDNAME";
        combo.DataValueField = "FLDFLAGPERFORMANCEID";
        combo.DataBind();
        combo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

                if (ucFlag.SelectedFlag == "")
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
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUShipFlagPerformanceColorInsert(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                         General.GetNullableInteger(ucFlag.SelectedFlag),
                                                                         General.GetNullableInteger(ddlflagperf.SelectedValue),
                                                                         General.GetNullableGuid(mou)                                                                         
                                                                         //ref colorscoreid
                                                                         );

                                }

                            }
                        }
                    }
                }
                if (ViewState["FLDFLAGID"] != null && ViewState["ACTION"].ToString() == "edit")
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
                                    PhoenixInspectionPSCMOUMatrix.PSCMOUShipFlagPerformanceColorUpdate(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                         General.GetNullableInteger(ucFlag.SelectedFlag),
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