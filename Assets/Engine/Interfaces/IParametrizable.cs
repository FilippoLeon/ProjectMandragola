using UnityEngine;

public interface IParametrizable
{
    Vector2 parameterRange(string param);
    bool parameterHasRange(string param);
    bool hasParameter(string name);
    void setParameter<T>(string name, T value);
}