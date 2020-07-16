

//    /// <summary>
//    /// 添加监听
//    /// </summary>
//    protected override void AddListener()
//    {
//        EventListener.instance.AddEvent<float, float, float>("UpdateProgress", UpdateProgress);
//        EventListener.instance.AddEvent<string>("SetTips", SetTips);
//    }

//    /// <summary>
//    /// 移除监听
//    /// </summary>
//    protected override void RemoveListener()
//    {
//        EventListener.instance.RemoveEvent<float, float, float>("UpdateProgress", UpdateProgress);
//        EventListener.instance.RemoveEvent<string>("SetTips", SetTips);
//    }

//    /// <summary>
//    /// 更新进度
//    /// </summary>
//    /// <param name="curSize"></param>
//    /// <param name="totalSize"></param>
//    /// <param name="speed"></param>
//    private void UpdateProgress(float curSize, float totalSize, float speed)
//    {
//        GetComponent<Slider>("Slider").value = curSize / totalSize;
//        GetComponent<Text>("Details").text = ConfigManager.GetLangFormat("UpdateDetail", "", curSize, totalSize, speed);
//    }

//    /// <summary>
//    /// 小提示
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator StartTips()
//    {
//        string[] tips = ConfigManager.GetLang("SmallTips").Split(';');
//        while (true)
//        {
//            GetComponent<Text>("Tips").text = tips[Random.Range(0, tips.Length)];
//            yield return new WaitForSeconds(m_tipsTime);
//        }
//    }

//    /// <summary>
//    /// 设置提示内容
//    /// </summary>
//    /// <param name="tips"></param>
//    private void SetTips(string tips)
//    {
//        if (null == tips)
//        {
//            StopCoroutine("StartTips");
//            StartCoroutine("StartTips");
//        }
//        else
//        {
//            GetComponent<Text>("Tips").text = tips;
//        }
//    }
//    #endregion
//}
