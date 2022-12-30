using System;
using System.Collections.Generic;

[Serializable]
public class JSONdata 
{
    public string current_page;
    public List<Entry> entries;
}
[Serializable]
public class Entry 
{
    public string id;
    public string name;
    public string role;

}

[Serializable]
public class loadingimg
{
    public string image;
}