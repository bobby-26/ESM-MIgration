using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

public partial class DocumentManagementFormPreview : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["FORMREVISIONID"] != null && Request.QueryString["FORMREVISIONID"].ToString() != "")
                    ViewState["FORMREVISIONID"] = Request.QueryString["FORMREVISIONID"].ToString();
                else
                    ViewState["FORMREVISIONID"] = "";

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                else
                    ViewState["FORMID"] = "";
                ViewState["FORMNAME"] = "";
            }
            GetFormDetails();
            BindField();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindField()
    {
        if (ViewState["FORMID"] != null && ViewState["FORMREVISIONID"] != null && General.GetNullableGuid(ViewState["FORMID"].ToString()) != null && General.GetNullableGuid(ViewState["FORMREVISIONID"].ToString()) != null)
        {
            DataSet dsData = new DataSet();

            DataSet ds = PhoenixDocumentManagementForm.FormFieldList(
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["FORMID"].ToString())
                        , new Guid(ViewState["FORMREVISIONID"].ToString()));


            HtmlTable tblTitle = new HtmlTable();
            tblTitle.Width = "100%";
            HtmlTableRow trTitle = new HtmlTableRow();
            HtmlTableCell tdTitle = new HtmlTableCell();

            Label lbltest = new Label();
            lbltest.Text = ViewState["FORMNAME"].ToString() + "<br><br><br>";
            lbltest.Font.Size = new FontUnit(15);
            lbltest.Font.Underline = true;
            tdTitle.ColSpan = 4;
            tdTitle.Controls.Add(lbltest);
            tdTitle.Align = "Center";
            trTitle.Cells.Add(tdTitle);
            trTitle.Align = "Center";
            tblTitle.Rows.Add(trTitle);
            divForm.Controls.Add(tblTitle);
           
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td = new HtmlTableCell();

            int maxrowno = 0;
            if (ds.Tables[0].Rows.Count > 0)
                maxrowno = int.Parse(ds.Tables[0].Rows[0]["FLDMAXROWNO"].ToString());
            
            string tdstyle = "";            
            int colspan = 1;

            colspan = int.Parse(ds.Tables[0].Rows[0]["FLDMAXCOLCOUNT"].ToString());
//            colspan = colspan * 2;
            

            for (int i = 0; i < maxrowno; i++)
            {
                string previousserialno = "";                

                DataRow[] drColl = ds.Tables[0].Select("FLDROWNO =" + (i + 1), "FLDSERIALNUMBER");                              

                tr = new HtmlTableRow();

                foreach (DataRow dr in drColl)
                {
                    if (General.GetNullableDecimal(previousserialno) == null)
                    {
                        previousserialno = dr["FLDSERIALNUMBER"].ToString();
                        td = new HtmlTableCell();
                    }
                    else
                    {
                        if (previousserialno == dr["FLDSERIALNUMBER"].ToString())
                        {                            
                            td.Controls.Add(new LiteralControl("&nbsp;"));
                        }
                        else
                        {
                            previousserialno = dr["FLDSERIALNUMBER"].ToString();
                            td = new HtmlTableCell();
                        }
                    }

                    ViewState["FIELDID"] = dr["FLDFIELDID"].ToString();
                    if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "TABLE")
                    {
                        tr = new HtmlTableRow();
                        td = new HtmlTableCell();

                        HtmlTable tbl = new HtmlTable();
                        HtmlTableCell tbltd = new HtmlTableCell();
                        HtmlTableRow tbltr = new HtmlTableRow();
                        int n = int.Parse(dr["FLDROWS"].ToString());
                        int m = int.Parse(dr["FLDCOLUMNS"].ToString());

                        for (int rows = 0; rows < n; rows++)
                        {
                            tbltr = new HtmlTableRow();
                            for (int cols = 0; cols < m; cols++)
                            {
                                tbltd = new HtmlTableCell();
                                tbltd.Attributes.Add("style", "border:1px solid black;border-collapse:collapse;empty-cells: show");
                                tbltr.Cells.Add(tbltd);
                            }
                            tbl.Rows.Add(tbltr);
                        }
                        tbl.Attributes.Add("style", "border:1px solid black;border-collapse:collapse;");
                        tbl.Width = "100%";

                        td.Controls.Add(tbl);
                        td.Height = "100%";
                        td.Width = "100%";
                        td.ColSpan = 4;
                        tr.Cells.Add(td);
                        tblForm.Rows.Add(tr);

                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "TEXTBOX")
                    {
                        //td = new HtmlTableCell();
                        //td.Attributes.Add("Style", tdstyle);
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);
                        
                        //td = new HtmlTableCell();
                        td.Attributes.Add("Style", tdstyle);
                        TextBox tb = new TextBox();
                        tb.Width = new Unit(dr["FLDSIZE"].ToString());
                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            tb.CssClass = "input_mandatory";
                        else
                            tb.CssClass = "input";
                        
                        td.Controls.Add(tb);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "PARAGRAPH TEXT")
                    {
                        //td = new HtmlTableCell();
                        
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;                       

                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);                        

                        //td = new HtmlTableCell();
                        TextBox tb = new TextBox();
                        tb.TextMode = TextBoxMode.MultiLine;
                        tb.Rows = 5;
                        tb.Width = new Unit(dr["FLDSIZE"].ToString());
                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            tb.CssClass = "input_mandatory";
                        else
                            tb.CssClass = "input";                      

                        td.Controls.Add(tb);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "CAPTION")
                    {
                        td = new HtmlTableCell();                        
                        Label lbl = new Label();
                        lbl.Text = dr["FLDLABEL"].ToString();
                        if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                            lbl.Font.Bold = true;
                        td.Controls.Add(lbl);
                        tr.Cells.Add(td);

                        //td = new HtmlTableCell();
                        //td.Attributes.Add("Style", tdstyle);
                        //tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "HEADLINE NORMAL")
                    {
                        td = new HtmlTableCell();                        
                        Label lbl = new Label();
                        lbl.Text = dr["FLDLABEL"].ToString() + "</br>";
                        lbl.Font.Size = new FontUnit(11);
                        if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                            lbl.Font.Bold = true;

                        td.ColSpan = colspan;
                        td.Controls.Add(lbl);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "NUMBER")
                    {
                        //td = new HtmlTableCell();                        
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();                        
                        UserControlMaskNumber ucMaskNumber = (UserControlMaskNumber)this.LoadControl("../UserControls/UserControlMaskNumber.ascx");
                        ucMaskNumber.Width = new Unit(dr["FLDSIZE"].ToString());
                        ucMaskNumber.IsPositive = true;
                        ucMaskNumber.IsInteger = true;
                        ucMaskNumber.DecimalPlace = 0;
                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            ucMaskNumber.CssClass = "input_mandatory txtNumber";
                        else
                            ucMaskNumber.CssClass = "input";
                        if (General.GetNullableInteger(dr["FLDPRECISION"].ToString()) != null)
                            ucMaskNumber.MaxLength = int.Parse(dr["FLDPRECISION"].ToString());

                        td.Controls.Add(ucMaskNumber);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "PHONE")
                    {
                        //td = new HtmlTableCell();
                        
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();                        
                        UserControlPhoneNumber PhoneNumber = (UserControlPhoneNumber)this.LoadControl("../UserControls/UserControlPhoneNumber.ascx");
                        PhoneNumber.Width = dr["FLDSIZE"].ToString();

                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            PhoneNumber.CssClass = "input_mandatory";
                        else
                            PhoneNumber.CssClass = "input";

                        td.Controls.Add(PhoneNumber);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "CHECKBOX")
                    {
                        //td = new HtmlTableCell();
                        
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();                        
                        CheckBox cbox = new CheckBox();
                        td.Controls.Add(cbox);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "SEPERATOR")
                    {
                        td = new HtmlTableCell();                        
                        td.InnerHtml = "<hr>";
                        td.ColSpan = colspan;
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "NOTE")
                    {
                        tr = new HtmlTableRow();
                        td = new HtmlTableCell();
                        if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                            td.InnerHtml = "<b>" + dr["FLDLABEL"].ToString() + "</b>";
                        else
                            td.InnerHtml = dr["FLDLABEL"].ToString();
                        td.ColSpan = colspan;
                        tr.Cells.Add(td);                        
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "LINE BREAK")
                    {
                        td = new HtmlTableCell();
                        td.InnerHtml = "<br/>";                        
                        tr.Cells.Add(td);                      
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "PRICE")
                    {
                        //td = new HtmlTableCell();

                        //td.Attributes.Add("Style", tdstyle);
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();
                        td.Attributes.Add("Style", tdstyle);
                        UserControlMaskNumber Price = (UserControlMaskNumber)this.LoadControl("../UserControls/UserControlMaskNumber.ascx");
                        Price.Width = new Unit(dr["FLDSIZE"].ToString());

                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            Price.CssClass = "input_mandatory";
                        else
                            Price.CssClass = "input";

                        if (General.GetNullableInteger(dr["FLDPRECISION"].ToString()) != null)
                            Price.MaxLength = int.Parse(dr["FLDPRECISION"].ToString());
                        if (General.GetNullableInteger(dr["FLDSCALE"].ToString()) != null)
                            Price.DecimalPlace = int.Parse(dr["FLDSCALE"].ToString());

                        td.Controls.Add(Price);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "DATE")
                    {
                        //td = new HtmlTableCell();
                        //td.Attributes.Add("Style", tdstyle);
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();
                        td.Attributes.Add("Style", tdstyle);
                        UserControlDate Date = (UserControlDate)this.LoadControl("../UserControls/UserControlDate.ascx");

                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            Date.CssClass = "input_mandatory";
                        else
                            Date.CssClass = "input";
                        Date.DatePicker = true;

                        td.Controls.Add(Date);
                        tr.Cells.Add(td);
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "DROPDOWN")
                    {
                        //td = new HtmlTableCell();

                        //td.Attributes.Add("Style", tdstyle);
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();
                        td.Attributes.Add("Style", tdstyle);
                        DropDownList ddlList = new DropDownList();
                        ddlList.Width = new Unit(dr["FLDSIZE"].ToString());
                        if (General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) != null && General.GetNullableInteger(dr["FLDMANDATORY"].ToString()) == 1)
                            ddlList.CssClass = "input_mandatory";
                        else
                            ddlList.CssClass = "input";
                        td.Controls.Add(ddlList);
                        tr.Cells.Add(td);

                        dsData = GetChoiceList(new Guid(dr["FLDFIELDID"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlList.DataSource = dsData;
                            ddlList.DataTextField = "FLDTEXT";
                            ddlList.DataValueField = "FLDVALUE";
                            ddlList.DataBind();

                            ddlList.Items.Insert(0, new ListItem("--Select--", ""));
                        }
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "MULTIPLE CHOICE")
                    {
                        //td = new HtmlTableCell();
                        //td.Attributes.Add("Style", tdstyle);
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;
                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();
                        td.Attributes.Add("Style", tdstyle);
                        RadioButtonList rlist = new RadioButtonList();
                        rlist.Width = new Unit(dr["FLDSIZE"].ToString());
                        td.Controls.Add(rlist);
                        tr.Cells.Add(td);

                        dsData = GetChoiceList(new Guid(dr["FLDFIELDID"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            rlist.DataSource = dsData;
                            rlist.DataTextField = "FLDTEXT";
                            rlist.DataValueField = "FLDVALUE";
                            rlist.DataBind();
                            rlist.ValidationGroup = "DMS";

                            rlist.CellSpacing = 2;
                            rlist.CellPadding = 2;
                            rlist.RepeatDirection = RepeatDirection.Horizontal;
                        }
                    }
                    else if (General.GetNullableString(dr["FLDTYPE"].ToString()) == "IMAGE")
                    {
                        //td = new HtmlTableCell();
                        //td.Attributes.Add("Style", tdstyle);
                        //Label lbl = new Label();
                        //lbl.Text = dr["FLDLABEL"].ToString();
                        //if (General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) != null && General.GetNullableInteger(dr["FLDBOLDYN"].ToString()) == 1)
                        //    lbl.Font.Bold = true;

                        //td.Controls.Add(lbl);
                        //tr.Cells.Add(td);

                        //td = new HtmlTableCell();
                        td.Attributes.Add("Style", tdstyle);
                        //HtmlImage img = new HtmlImage();
                        Image img = new Image();                       
                        img.Width = int.Parse(dr["FLDSIZE"].ToString());
                        img.Height = int.Parse(dr["FLDHEIGHT"].ToString());
                        string strImgId = Guid.NewGuid().ToString();
                        img.ID = strImgId;                                         

                        Button btnUpload = new Button();
                        btnUpload.Text = "Upload";
                        btnUpload.Click += new EventHandler(this.btnUpload_Click);                            
                        
                        HtmlInputFile ifile = new HtmlInputFile();
                        ifile.Attributes.Add("runat", "server");
                        ifile.Attributes.Add("onchange", @"getUploadedFile(this,""" + strImgId + @""")");
                        
                        td.Controls.Add(img);
                        td.Controls.Add(ifile);
                        //td.Controls.Add(btnUpload);                        
                        tr.Cells.Add(td);
                    }
                }               
                tblForm.Rows.Add(tr);                
                divForm.Controls.Add(tblForm);
            }
        }
    }

    protected void btnUpload_Click(object sender, System.EventArgs e)
    {
        Button btn = (Button)sender;
        
        foreach (HtmlTableRow R in tblForm.Rows)
        {
            foreach (HtmlTableCell C in R.Cells)
            {
                if (C.Controls.Contains(btn))
                {
                    Image image = (Image)C.Controls[0];
                    HtmlInputFile ifileupload = (HtmlInputFile)C.Controls[1];

                    if (ifileupload != null && image != null)
                    {
                        if (ifileupload.Value != "")                
                        {
                            try
                            {
                                string imgpath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                                //imgpath = imgpath + "\\" + ifileupload.FileName;
                                //ifileupload.SaveAs(imgpath);
                                image.ImageUrl = imgpath;
                                //Label1.Text = "Uploaded File Name: " +
                                //    FileUpload1.PostedFile.FileName +
                                //    "<br />File Size: " +
                                //    FileUpload1.PostedFile.ContentLength +
                                //    "<br />File Type: " +
                                //    FileUpload1.PostedFile.ContentType;                               
                            }
                            catch (Exception ex)
                            {
                                ucError.Text = "Error occured: " + ex.Message.ToString();
                            }
                        }
                        else
                        {
                            ucError.Text = "Select a file for upload";
                        }
                    }
                }
            }
        }        
    }

    protected DataSet GetChoiceList(Guid fieldid)
    {
        DataSet ds = PhoenixDocumentManagementForm.FormFieldChoiceList(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["FORMID"].ToString())
                                                            , new Guid(ViewState["FORMREVISIONID"].ToString())
                                                            , fieldid);
        return ds;
    }

    private void GetFormDetails()
    {
        if (ViewState["FORMID"] != null && ViewState["FORMID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementForm.FormEdit(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["FORMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ttlContent.Text = dr["FLDCAPTION"].ToString() + " - " + "Preview";
                ViewState["FORMNAME"] = dr["FLDCAPTION"].ToString();
            }
        }
    }    
}
