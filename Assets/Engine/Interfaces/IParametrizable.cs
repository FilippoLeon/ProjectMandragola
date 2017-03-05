﻿using UnityEngine;

public interface IParametrizable
{
    Vector2 parameterRange(string param);
    bool parameterHasRange(string param);
    void setParameter<T>(string name, T value);
}