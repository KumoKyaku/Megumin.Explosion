using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    //方法名字中带有有On 例如OnClickd()，OnValueChanged()，OnKeyChanged()
    //委托声明带有On 例如public delegate void OnClicked();
    //委托参数形参声明带有On，因为代表方法 例如 public void Regist(OnChanged<int> onChangedCallback)
    //事件声明不带On/on
    //  例如
    //  public event OnClicked Clicked;
    //  public event OnChanged<int> ValueChanged;
    //  public event OnChanged<int> KeyChanged;


    /// <summary>
    /// 当值被访问时被调用。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <remarks>
    /// 保持API一致性。<see cref="OnValueSet{T}"/>
    /// 虽然应该是Got，但是为了一致性。
    /// </remarks>
    [Obsolete("Use OnGet instead.")]
    public delegate void OnValueGet<in T>(T value);

    /// <summary>
    /// 当值被赋值后被调用，无论新值是否与旧值相等。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newValue"></param>
    /// <param name="oldValue"></param>
    /// <remarks>
    /// Set过去式也是Set。
    /// </remarks>
    [Obsolete("Use OnSet instead.")]
    public delegate void OnValueSet<in T>(T newValue, T oldValue);

    /// <summary>
    /// 当值发生改变后被调用。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newValue">新的值</param>
    /// <param name="oldValue">旧的值</param>
    [Obsolete("Use OnChanged instead.")]
    public delegate void OnValueChanged<in T>(T newValue, T oldValue);

    /// <summary>
    /// 当值被访问时被调用。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <remarks>
    /// 保持API一致性。<see cref="OnSet{T}"/>
    /// 虽然应该是Got，但是为了一致性。
    /// </remarks>
    public delegate void OnGet<in T>(T value);

    /// <summary>
    /// 当值被赋值后被调用，无论新值是否与旧值相等。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newValue"></param>
    /// <param name="oldValue"></param>
    /// <remarks>
    /// Set过去式也是Set。
    /// </remarks>
    public delegate void OnSet<in T>(T newValue, T oldValue);

    /// <summary>
    /// 当值发生改变后被调用。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newValue">新的值</param>
    /// <param name="oldValue">旧的值</param>
    public delegate void OnChanged<in T>(T newValue, T oldValue);
}
